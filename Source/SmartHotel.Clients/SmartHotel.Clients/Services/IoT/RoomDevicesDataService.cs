using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SmartHotel.Clients.Core.Extensions;
using SmartHotel.Clients.Core.Helpers;
using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Services.Authentication;
using SmartHotel.Clients.Core.Services.Request;

namespace SmartHotel.Clients.Core.Services.IoT
{
    public class RoomDevicesDataService : IRoomDevicesDataService
    {
        private readonly TimeSpan _sensorDataPollingInterval = TimeSpan.FromSeconds(3);
        private readonly string _roomDevicesApiEndpoint;
        private Timer _sensorDataPollingTimer;

        private readonly IRequestService _requestService;
        private readonly IAuthenticationService _authenticationService;

        private readonly ConcurrentDictionary<SensorDataType, DeviceSensorData>
            _currentSensorDataBySensorDataType = new ConcurrentDictionary<SensorDataType, DeviceSensorData>();
        private readonly List<DeviceDesiredData> _desiredData = new List<DeviceDesiredData>();

        private readonly string _roomId;

	    private int _startPollingRequestCount = 0;
		
        public RoomDevicesDataService(IRequestService requestService, IAuthenticationService authenticationService)
        {
            _requestService = requestService;
            _authenticationService = authenticationService;
            _roomDevicesApiEndpoint = AppSettings.RoomDevicesEndpoint;

            if (!UseFakes)
            {
                if (string.IsNullOrWhiteSpace(AppSettings.RoomId))
                {
                    throw new Exception($"{nameof(AppSettings)}.{nameof(AppSettings.RoomId)} must be specified.");
                }
                _roomId = AppSettings.RoomId;

            }
        }

		public event EventHandler SensorDataChanged;
		private void OnSensorDataChanged()
		{
			SensorDataChanged?.Invoke( this, EventArgs.Empty );
		}

        public bool UseFakes => string.IsNullOrWhiteSpace(_roomDevicesApiEndpoint);

        public bool IsPollingData => _sensorDataPollingTimer != null && _sensorDataPollingTimer.IsRunning;

        public async Task<RoomTemperature> GetRoomTemperatureAsync()
        {
            if (UseFakes)
            {
                await Task.Delay(1000);

                return FakeRoomTemperature.Create();
            }

			var storedValue = GetStoredSensorData<RoomTemperature>();
			if ( storedValue != null )
			{
				return (RoomTemperature)storedValue;
			}

            await GetLatestData();

			return (RoomTemperature)GetStoredSensorData<RoomTemperature>();
        }

		public async Task<RoomAmbientLight> GetRoomAmbientLightAsync()
        {
            if (UseFakes)
            {
                await Task.Delay(1000);

                return FakeRoomAmbientLight.Create();
            }

            var storedValue = GetStoredSensorData<RoomAmbientLight>();
            if (storedValue != null)
            {
                return (RoomAmbientLight)storedValue;
            }

            await GetLatestData();

            return (RoomAmbientLight)GetStoredSensorData<RoomAmbientLight>();
        }


        private RoomSensorBase GetStoredSensorData<T>() where T : RoomSensorBase, new()
        {
            RoomSensorBase sensor = new T();
            if (_currentSensorDataBySensorDataType.TryGetValue(sensor.SensorDataType,
                out var sensorData))
            {
                var desired = _desiredData.FirstOrDefault(d => d.RoomId == sensorData.RoomId && d.SensorId == sensorData.SensorId);

                var currentTemp = float.Parse(sensorData.SensorReading);
                var desiredTemp = float.Parse(desired != null ? desired.DesiredValue : sensorData.SensorReading);

                if (typeof(T) == typeof(RoomTemperature))
                    return new RoomTemperature(new SensorValue(currentTemp), new SensorValue(desiredTemp));

                if (typeof(T) == typeof(RoomAmbientLight))
                    return new RoomAmbientLight(new SensorValue(currentTemp * 100f), new SensorValue(desiredTemp * 100f));
            }

            return null;
        }

        private void ProcessRoomData(IEnumerable<DeviceSensorData> data, IEnumerable<DeviceDesiredData> desired)
        {
            foreach (var rawSensor in data)
            {
                if (Enum.TryParse(rawSensor.SensorDataType, out SensorDataType dataType))
                    _currentSensorDataBySensorDataType.AddOrUpdate(dataType, rawSensor, (key, oldValue) => rawSensor);
            }

            if (desired != null && desired.ToList().Count > 0)
            {
                _desiredData.Clear();
                _desiredData.AddRange(desired);
            }
        }

        private async Task<IEnumerable<DeviceSensorData>> GetRoomSensorData(string token, string roomId)
        {
            var builder = new UriBuilder(_roomDevicesApiEndpoint);
            builder.AppendToPath($"Devices/room/{roomId}");
            var uri = builder.ToString();

            return await _requestService.GetAsync<IEnumerable<DeviceSensorData>>(uri, token);
        }

        private async Task<IEnumerable<DeviceDesiredData>> GetRoomDesiredData(string token, string roomId)
        {
            var builder = new UriBuilder(_roomDevicesApiEndpoint);
            builder.AppendToPath($"Devices/desired/{roomId}");
            var uri = builder.ToString();

            return await _requestService.GetAsync<IEnumerable<DeviceDesiredData>>(uri, token);
        }

        public async Task UpdateDesiredAsync(float desiredTemperature, SensorDataType sensorDataType)
        {
            if (!_currentSensorDataBySensorDataType.TryGetValue(sensorDataType, out DeviceSensorData deviceData))
                return;

            var builder = new UriBuilder(_roomDevicesApiEndpoint);
            builder.AppendToPath("Devices");
            var uri = builder.ToString();

            string methodName;
            switch (sensorDataType)
            {
                case SensorDataType.Temperature:
                    methodName = "SetDesiredTemperature";
                    break;
                case SensorDataType.Light:
                    methodName = "SetDesiredAmbientLight";
                    break;
                default:
                    throw new NotSupportedException(sensorDataType.ToString());
            }

            var request = new DeviceRequest
            {
                RoomId = deviceData.RoomId,
                SensorId = deviceData.SensorId,
                DeviceId = deviceData.IoTHubDeviceId,
                MethodName = methodName,
                Value = desiredTemperature.ToString(CultureInfo.InvariantCulture)
            };

            await _requestService.PostAsync(uri, request, _authenticationService.AuthenticatedUser.Token);
        }

        public void StartCheckingRoomSensorData()
        {
            if (UseFakes)
            {
                return;
            }

	        if (_sensorDataPollingTimer == null)
	        {
		        _sensorDataPollingTimer = new Timer(_sensorDataPollingInterval, SensorDataPollingTimerTick);
		        _sensorDataPollingTimer.Start();
	        }

	        _startPollingRequestCount++;
        }

        public void StopCheckingRoomSensorData()
        {
	        _startPollingRequestCount--;
	        if (_startPollingRequestCount <= 0)
	        {
				// Last request to stop polling
		        _startPollingRequestCount = 0;
		        _sensorDataPollingTimer?.Stop();
		        _sensorDataPollingTimer = null;
	        }
        }

        private async void SensorDataPollingTimerTick()
        {
            _sensorDataPollingTimer?.Stop();

            await GetLatestData();
	        OnSensorDataChanged();

            _sensorDataPollingTimer?.Start();
        }

        private async Task GetLatestData()
        {
            var roomData = await GetRoomSensorData(_authenticationService.AuthenticatedUser.Token, _roomId);
            var desiredData = await GetRoomDesiredData(_authenticationService.AuthenticatedUser.Token, _roomId);
            ProcessRoomData(roomData, desiredData);
        }
    }
}