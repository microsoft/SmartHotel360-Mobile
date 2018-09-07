namespace SmartHotel.Clients.Core.Services.IoT
{
    public class RoomTemperature : RoomTemperatureBase
    {
        private string _temperatureSensorId;

        private static readonly TemperatureValue DefaultMinimum = new TemperatureValue(60);
        private static readonly TemperatureValue DefaultMaximum = new TemperatureValue(90);
        private static readonly TemperatureValue DefaultValue = new TemperatureValue(71);

        private RoomTemperature(TemperatureValue desired) : base(DefaultValue, DefaultMinimum, DefaultMaximum)
        {
            Desired = desired;
        }

        public RoomTemperature() : this(DefaultValue)
        {
        }

        public RoomTemperature(string temperatureSensorId, TemperatureValue desired) : this(temperatureSensorId, DefaultValue, desired)
        {
            _temperatureSensorId = temperatureSensorId;
        }

        public RoomTemperature(string temperatureSensorId, TemperatureValue value, TemperatureValue desired) : base(value, desired, DefaultMinimum, DefaultMaximum)
        {
            _temperatureSensorId = temperatureSensorId;
        }

    }
}