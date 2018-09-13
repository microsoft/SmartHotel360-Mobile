using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartHotel.Clients.Core.Extensions;
using SmartHotel.Clients.Core.Helpers;
using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Services.Request;

namespace SmartHotel.Clients.Core.Services.IoT
{
	public class RoomDevicesDataService : IRoomDevicesDataService
	{
	    // TODO: Probably best to get this in AppSettings
		private readonly TimeSpan _sensorDataPollingInterval = TimeSpan.FromSeconds( 5 );
		private readonly string _roomDevicesApiEndpoint;
		private readonly string _roomId;
		private Timer _sensorDataPollingTimer;

	    private readonly IRequestService _requestService; 

        private readonly ConcurrentDictionary<string, DeviceSensorData>
			_currentSensorDataBySensorDataType = new ConcurrentDictionary<string, DeviceSensorData>();

		public RoomDevicesDataService(IRequestService requestService)
		{
		    _requestService = requestService;
            _roomDevicesApiEndpoint = AppSettings.RoomDevicesEndpoint;
			_roomId = AppSettings.RoomId;
		}

		public bool UseFakes => string.IsNullOrEmpty( _roomDevicesApiEndpoint );

		public async Task<RoomTemperature> GetRoomTemperatureAsync(string token = "")
		{
			if ( UseFakes )
			{
				await Task.Delay( 1000 );

				return FakeRoomTemperature.Create();
			}
			else
			{
				if ( _currentSensorDataBySensorDataType.TryGetValue( RoomTemperature.SensorDataType,
					out DeviceSensorData sensorData ) )
				{
					var currentTemp = float.Parse( sensorData.SensorReading );
					var desiredTemp = float.Parse( sensorData.DesiredValue );
					return new RoomTemperature( new SensorValue( currentTemp ), new SensorValue( desiredTemp ) );
				}
				else
				{
                    var roomData = await RoomSensorData(token, _roomId);

                    // TODO: more generic approach to remove duplicated code in GetRoomAmbientLightAsync()
                    RoomTemperature temperature = null;
				    foreach (var sensor in roomData)
				    {
				        _currentSensorDataBySensorDataType.AddOrUpdate(sensor.SensorDataType, sensor, (key, oldValue) => sensor);
				        if (sensor.SensorDataType == RoomTemperature.SensorDataType)
				        {
				            var currentTemp = Convert.ToSingle(sensor.SensorReading);
				            var desiredTemp = Convert.ToSingle(sensor.DesiredValue);
				            temperature = new RoomTemperature(new SensorValue(currentTemp), new SensorValue(desiredTemp));
				        }
				    }

				    return temperature;
				}
			}
		}

	    private async Task<IEnumerable<DeviceSensorData>> RoomSensorData(string token, string roomId)
	    {
	        UriBuilder builder = new UriBuilder(_roomDevicesApiEndpoint);
	        builder.AppendToPath($"Devices/room/{roomId}");
	        var uri = builder.ToString();

            return await _requestService.GetAsync<IEnumerable<DeviceSensorData>> (uri, token);
	    }

		public async Task<RoomAmbientLight> GetRoomAmbientLightAsync(string token = "")
		{
			if ( UseFakes )
			{
				await Task.Delay( 1000 );

				return FakeRoomAmbientLight.Create();
			}
			else
			{
				if ( _currentSensorDataBySensorDataType.TryGetValue( RoomAmbientLight.SensorDataType,
					out DeviceSensorData sensorData ) )
				{
					var currentLight = new SensorValue( float.Parse( sensorData.SensorReading) * 100f, SensorTypes.DimmerSwitch );
					var desiredLight = new SensorValue( float.Parse( sensorData.DesiredValue) * 100f, SensorTypes.DimmerSwitch );
					return new RoomAmbientLight( currentLight, desiredLight );
				}
				else
				{
				    var roomData = await RoomSensorData(token, _roomId);

                    // TODO: more generic approach to remove duplicated code in GetRoomTemperatureAsync()
                    RoomAmbientLight light = null;
				    foreach (var sensor in roomData)
				    {
				        _currentSensorDataBySensorDataType.AddOrUpdate(sensor.SensorDataType, sensor, (key, oldValue) => sensor);
				        if (sensor.SensorDataType == RoomAmbientLight.SensorDataType)
				        {
				            var currentTemp = float.Parse(sensor.SensorReading);
				            var desiredTemp = float.Parse(sensor.DesiredValue);
				            light = new RoomAmbientLight(new SensorValue(currentTemp, SensorTypes.DimmerSwitch), new SensorValue(desiredTemp, SensorTypes.DimmerSwitch));
				        }
				    }

				    return light;
                }
			}
		}

		public Task UpdateDesiredRoomAmbientLightAsync( float desiredAmbientLight )
		{
			if ( _currentSensorDataBySensorDataType.TryGetValue( RoomAmbientLight.SensorDataType,
				out DeviceSensorData sensorData ) )
			{
				string sensorId = sensorData.SensorId;
				// TODO: Call the Room Devices api to change the desired ambient light.
			}
			throw new NotImplementedException();
		}

		public Task UpdateDesiredRoomTemperatureAsync( float desiredTemperature )
		{
			if ( _currentSensorDataBySensorDataType.TryGetValue( RoomTemperature.SensorDataType,
				out DeviceSensorData sensorData ) )
			{
				string sensorId = sensorData.SensorId;
				// TODO: Call the Room Devices api to change the desired temperature.
			}
			throw new NotImplementedException();
		}

		public void StartCheckingRoomSensorData()
		{
			if ( _sensorDataPollingTimer != null )
			{
				return;
			}

			_sensorDataPollingTimer = new Timer( _sensorDataPollingInterval, SensorDataPollingTimerTick );
			_sensorDataPollingTimer.Start();
		}

		public void StopCheckingRoomSensorData()
		{
			_sensorDataPollingTimer?.Stop();
			_sensorDataPollingTimer = null;
		}

		private void SensorDataPollingTimerTick()
		{
			_sensorDataPollingTimer?.Stop();

			// TODO: call IoT service
			IEnumerable<DeviceSensorData> results = null;
			if ( results != null )
			{
				foreach ( DeviceSensorData sensorData in results )
				{
					_currentSensorDataBySensorDataType[sensorData.SensorDataType] = sensorData;
				}
			}

			_sensorDataPollingTimer?.Start();
		}
	}
}