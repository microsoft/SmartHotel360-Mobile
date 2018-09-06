using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.IoT
{
    public interface ILiveIoTDataService
    {
        bool IsReadOnly { get; }

        Task<RoomAmbientLight> GetRoomAmbientLightAsync(string roomId);
        Task<RoomTemperature> GetRoomTemperatureAsync(string roomId);
    }
}