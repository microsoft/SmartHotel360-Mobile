namespace SmartHotel.Clients.Core.Services.IoT
{
	public class RoomAmbientLight : RoomSensorBase
	{
        public static readonly SensorValue DefaultMinimum = new SensorValue(0, SensorType.DimmerSwitch);
        public static readonly SensorValue DefaultMaximum = new SensorValue(100, SensorType.DimmerSwitch);

	    private static readonly SensorValue DefaultValue = new SensorValue( 80, SensorType.DimmerSwitch );

        public RoomAmbientLight( SensorValue desired ) : this(DefaultValue, desired)
        {
			Desired = desired;
		}

		public RoomAmbientLight() : this( DefaultValue )
		{
        }

		public RoomAmbientLight( SensorValue value, SensorValue desired ) : base( value, desired, DefaultMinimum, DefaultMaximum )
		{
		    SensorDataType = SensorDataType.Light;
        }


	}
}