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

                return new RoomTemperature(string.Empty);
            }
            else
            {
                // TODO: call IoT service
                return new RoomTemperature(string.Empty);
            }
        }

        
    }
}
