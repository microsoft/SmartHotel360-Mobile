namespace SmartHotel.Clients.Core.Services.IoT
{
    public static class FakeRoomAmbientLight
    {
        private static readonly TemperatureValue FakeDesiredValue = new TemperatureValue(3000, TemperatureTypes.Kelvin);

        public static RoomAmbientLight Create()
        {
            return new RoomAmbientLight(FakeDesiredValue);
        }
    }
}