using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHotel.Clients.Core.Helpers;
using SmartHotel.Clients.Core.Models;

namespace SmartHotel.Clients.Core.Services.IoT
{
	public class RoomDevicesDataService : IRoomDevicesDataService
	{
		// TODO: Probably best to get this in AppSettings
		private readonly TimeSpan _sensorDataPollingInterval = TimeSpan.FromSeconds( 5 );
		private readonly string _roomDevicesApiEndpoint;
		private readonly string _roomId;
		private Timer _sensorDataPollingTimer;

		private readonly ConcurrentDictionary<string, DeviceSensorData>
			_currentSensorDataBySensorDataType = new ConcurrentDictionary<string, DeviceSensorData>();

		public RoomDevicesDataService()
		{
			_roomDevicesApiEndpoint = AppSettings.RoomDevicesEndpoint;
			_roomId = AppSettings.RoomId;
		}

		public bool UseFakes => string.IsNullOrEmpty( _roomDevicesApiEndpoint );

		public async Task<RoomTemperature> GetRoomTemperatureAsync()
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
					return new RoomTemperature( new TemperatureValue( currentTemp ), new TemperatureValue( desiredTemp ) );
				}
				else
				{
					//TODO: Handle the case where we haven't actually retrieved data yet
					// simulates values returned for IoT device
					var currentTemp = 70;
					var desiredTemp = 76;
					return new RoomTemperature( new TemperatureValue( currentTemp ), new TemperatureValue( desiredTemp ) );
				}
			}
		}

		public async Task<RoomAmbientLight> GetRoomAmbientLightAsync()
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
					var currentLight = new TemperatureValue( float.Parse( sensorData.SensorReading ), TemperatureTypes.Kelvin );
					var desiredLight = new TemperatureValue( float.Parse( sensorData.DesiredValue ), TemperatureTypes.Kelvin );
					return new RoomAmbientLight( currentLight, desiredLight );
				}
				else
				{
					//TODO: Handle the case where we haven't actually retrieved data yet
					// simulates values returned for IoT device
					var currentLight = new TemperatureValue( 4500, TemperatureTypes.Kelvin );
					var desiredLight = new TemperatureValue( 4000, TemperatureTypes.Kelvin );
					return new RoomAmbientLight( currentLight, desiredLight );
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