using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.IoT
{
    public interface IRoomDevicesDataService
    {
        bool UseFakes { get; }

        Task<RoomAmbientLight> GetRoomAmbientLightAsync(string roomId);
        Task<RoomTemperature> GetRoomTemperatureAsync(string roomId);
    }
}