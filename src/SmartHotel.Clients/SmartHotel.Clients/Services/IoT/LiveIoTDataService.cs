using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.IoT
{
    public class LiveIoTDataService
    {
        private readonly string _roomDevicesApiEndpoint;

        public LiveIoTDataService(string roomDevicesApiEndpoint)
        {
            _roomDevicesApiEndpoint = roomDevicesApiEndpoint;
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
