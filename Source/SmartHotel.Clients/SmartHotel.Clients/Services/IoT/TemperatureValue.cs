namespace SmartHotel.Clients.Core.Services.IoT
{
    public struct SensorValue
    {
        public SensorValue(float value) : this(value, default(SensorType))
        {
        }

        public SensorValue(float rawValue, SensorType unit)
        {
            RawValue = rawValue;
            Unit = unit;
        }

        public SensorType Unit { get; private set; }
        public float RawValue { get; private set; }

    }
}