namespace SmartHotel.Clients.Core.Services.IoT
{
    public class RoomTemperature : RoomTemperatureBase
    {
        private string _temperatureSensorId;

        private static readonly TemperatureValue DefaultMinimum = new TemperatureValue(60);
        private static readonly TemperatureValue DefaultMaximum = new TemperatureValue(90);
        private static readonly TemperatureValue DefaultValue = new TemperatureValue(71);

        public RoomTemperature(): base(DefaultValue, DefaultMinimum, DefaultMaximum)
        {
            Desired = DefaultValue;
        }

        public RoomTemperature(string temperatureSensorId): this()
        {
            _temperatureSensorId = temperatureSensorId;
        }

        public TemperatureValue Desired { get; private set; }

    }
}