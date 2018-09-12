namespace SmartHotel.Clients.Core.Services.IoT
{
    public class RoomTemperature : RoomSensorBase
    {
	    public const string SensorDataType = "Temperature";
        public static readonly SensorValue DefaultMinimum = new SensorValue(60);
        public static readonly SensorValue DefaultMaximum = new SensorValue(90);

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