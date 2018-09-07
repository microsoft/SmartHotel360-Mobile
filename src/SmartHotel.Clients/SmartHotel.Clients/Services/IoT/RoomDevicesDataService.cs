using System;
using System.Threading.Tasks;
using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Services.Settings;

namespace SmartHotel.Clients.Core.Services.IoT
{
    public class RoomDevicesDataService : IRoomDevicesDataService
    {
        private readonly string _roomDevicesApiEndpoint;

        public RoomDevicesDataService()
        {
            _roomDevicesApiEndpoint = AppSettings.RoomDevicesEndpoint;
        }

        public bool UseFakes => string.IsNullOrEmpty(_roomDevicesApiEndpoint);

        public async Task<RoomTemperature> GetRoomTemperatureAsync(string roomId)
        {
            if (UseFakes)
            {
                await Task.Delay(1000);

                return FakeRoomTemperature.Create();
            }
            else
            {
                // TODO: call IoT service
                
                // simulates values returned for IoT device
                var temperatureSensorId = string.Empty;
                var currentTemp = 70;
                var desiredTemp = 76;
                return new RoomTemperature(temperatureSensorId, new TemperatureValue(currentTemp), new TemperatureValue(desiredTemp));

            }
        }

        public async Task<RoomAmbientLight> GetRoomAmbientLightAsync(string roomId)
        {
            if (UseFakes)
            {
                await Task.Delay(1000);

                return FakeRoomAmbientLight.Create();
            }
            else
            {
                // TODO: call IoT service
                
                // simulates values returned for IoT device
                var _lightSensorId = string.Empty;
                var currentLight = new TemperatureValue(4500, TemperatureTypes.Kelvin); 
                var desiredLight = new TemperatureValue(4000, TemperatureTypes.Kelvin); ;
                return new RoomAmbientLight(_lightSensorId, currentLight, desiredLight);
            }
        }


    }

}
