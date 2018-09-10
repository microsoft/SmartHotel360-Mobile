using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.IoT
{
	public interface IRoomDevicesDataService
	{
		bool UseFakes { get; }

		Task<RoomAmbientLight> GetRoomAmbientLightAsync();
		Task<RoomTemperature> GetRoomTemperatureAsync();
		Task UpdateDesiredRoomAmbientLightAsync( float desiredAmbientLight );
		Task UpdateDesiredRoomTemperatureAsync( float desiredTemperature );
		void StartCheckingRoomSensorData();
		void StopCheckingRoomSensorData();
	}
}