using System.Threading.Tasks;
using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Services.Settings;

namespace SmartHotel.Clients.Core.Services.IoT
{
    public class LiveIoTDataService : ILiveIoTDataService
    {
        private readonly string _roomDevicesApiEndpoint;

        public LiveIoTDataService()
        {
            _roomDevicesApiEndpoint = AppSettings.RoomDevicesBaseUri;
        }

        public bool IsReadOnly => string.IsNullOrEmpty(_roomDevicesApiEndpoint);

        public async Task<RoomTemperature> GetRoomTemperatureAsync(string roomId)
        {
            if (IsReadOnly)
            {
                await Task.Delay(1000);

                return new RoomTemperature();
            }
            else
            {
                // TODO: call IoT service
                return new RoomTemperature(string.Empty);
            }
        }

        public async Task<RoomAmbientLight> GetRoomAmbientLightAsync(string roomId)
        {
            if (IsReadOnly)
            {
                await Task.Delay(1000);

                return new RoomAmbientLight();
            }
            else
            {
                // TODO: call IoT service
                return new RoomAmbientLight(string.Empty);
            }
        }


    }
}
