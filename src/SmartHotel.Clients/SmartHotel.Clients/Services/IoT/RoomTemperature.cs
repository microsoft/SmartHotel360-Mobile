namespace SmartHotel.Clients.Core.Services.IoT
{
    public class RoomTemperature : RoomTemperatureBase
    {
        private string _temperatureSensorId;

        private static readonly TemperatureValue DefaultMinimum = new TemperatureValue(60);
        private static readonly TemperatureValue DefaultMaximum = new TemperatureValue(90);
        private static readonly TemperatureValue DefaultValue = new TemperatureValue(71);
        private static readonly TemperatureValue FakeDesiredValue = new TemperatureValue(76);

        public static RoomTemperature CreateFake()
        {
            return new RoomTemperature(FakeDesiredValue);
        }

        private RoomTemperature(TemperatureValue desired): base(DefaultValue, DefaultMinimum, DefaultMaximum)
        {
            Desired = desired;
        }

        public RoomTemperature(string temperatureSensorId): this(temperatureSensorId, DefaultValue)
        {
        }

        public RoomTemperature(string temperatureSensorId, TemperatureValue value) : base(value, DefaultMinimum, DefaultMaximum)
        {
            _temperatureSensorId = temperatureSensorId;
        }

    }
}