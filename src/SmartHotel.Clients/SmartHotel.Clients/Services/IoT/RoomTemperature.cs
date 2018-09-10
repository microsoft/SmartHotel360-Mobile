namespace SmartHotel.Clients.Core.Services.IoT
{
    public class RoomTemperature : RoomTemperatureBase
    {
	    public const string SensorDataType = "Temperature";
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

        public RoomTemperature(TemperatureValue value, TemperatureValue desired) : base(value, desired, DefaultMinimum, DefaultMaximum)
        {
        }

    }
}