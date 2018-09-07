namespace SmartHotel.Clients.Core.Services.IoT
{
    public static class FakeRoomTemperature
    {
        private static readonly TemperatureValue FakeDesiredValue = new TemperatureValue(76);

        public static RoomTemperature Create()
        {
            return new RoomTemperature(string.Empty, FakeDesiredValue);
        }
    }
}