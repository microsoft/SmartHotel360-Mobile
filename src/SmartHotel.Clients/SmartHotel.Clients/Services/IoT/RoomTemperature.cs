namespace SmartHotel.Clients.Core.Services.IoT
{
    public class RoomTemperature : RoomTemperatureBase
    {
	    public const string SensorDataType = "Temperature";
        private static readonly SensorValue DefaultMinimum = new SensorValue(60);
        private static readonly SensorValue DefaultMaximum = new SensorValue(90);
        private static readonly SensorValue DefaultValue = new SensorValue(71);

        public RoomTemperature(SensorValue desired) : base(DefaultValue, DefaultMinimum, DefaultMaximum)
        {
            Desired = desired;
        }

        public RoomTemperature() : this(DefaultValue)
        {
        }

        public RoomTemperature(SensorValue value, SensorValue desired) : base(value, desired, DefaultMinimum, DefaultMaximum)
        {
        }

    }
}