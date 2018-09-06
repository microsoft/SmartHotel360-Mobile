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

                return RoomTemperature.CreateFake();
            }
            else
            {
                // TODO: call IoT service
                return new RoomTemperature(string.Empty);
            }
        }

        public async Task<RoomAmbientLight> GetRoomAmbientLightAsync(string roomId)
        {
            if (UseFakes)
            {
                await Task.Delay(1000);

                return RoomAmbientLight.CreateFake();
            }
            else
            {
                // TODO: call IoT service
                return new RoomAmbientLight(string.Empty);
            }
        }


    }

}
