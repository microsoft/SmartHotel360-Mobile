namespace SmartHotel.Clients.Core.Services.IoT
{
    public struct SensorValue
    {
        public SensorValue(float value) : this(value, default(SensorTypes))
        {
        }

        public SensorValue(float rawValue, SensorTypes unit)
        {
            RawValue = rawValue;
            Unit = unit;
        }

        public SensorTypes Unit { get; private set; }
        public float RawValue { get; private set; }

    }
}