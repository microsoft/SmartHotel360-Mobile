using System;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.IoT
{
    public interface IRoomDevicesDataService
    {
        bool UseFakes { get; }

        Task<RoomAmbientLight> GetRoomAmbientLightAsync();
        Task<RoomTemperature> GetRoomTemperatureAsync();
        Task UpdateDesiredAsync(float desiredTemperature, SensorDataType sensorDataType);
        void StartCheckingRoomSensorData(Action sensorDataChangedCallback);
        void StopCheckingRoomSensorData();
        bool IsPollingData { get; }
    }
}