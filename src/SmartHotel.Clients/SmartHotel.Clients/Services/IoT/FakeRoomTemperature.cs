namespace SmartHotel.Clients.Core.Services.IoT
{
    public static class FakeRoomTemperature
    {
        private static readonly SensorValue FakeDesiredValue = new SensorValue(76);

        public static RoomTemperature Create()
        {
            return new RoomTemperature(FakeDesiredValue);
        }
    }
}