namespace SmartHotel.Clients.Core.Services.IoT
{
	public class RoomAmbientLight : RoomTemperatureBase
	{
		public const string SensorDataType = "Light";
	    public static readonly SensorValue DefaultMinimum = new SensorValue(0, SensorTypes.DimmerSwitch);
        public static readonly SensorValue DefaultMaximum = new SensorValue(100, SensorTypes.DimmerSwitch);

	    private static readonly SensorValue DefaultValue = new SensorValue( 80, SensorTypes.DimmerSwitch );

		public RoomAmbientLight( SensorValue desired ) : base( DefaultValue, DefaultMinimum, DefaultMaximum )
		{
			Desired = desired;
		}

		public RoomAmbientLight() : this( DefaultValue )
		{
		}

		public RoomAmbientLight( SensorValue value, SensorValue desired ) : base( value, desired, DefaultMinimum, DefaultMaximum )
		{
		}


	}
}