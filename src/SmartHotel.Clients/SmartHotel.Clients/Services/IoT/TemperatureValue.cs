namespace SmartHotel.Clients.Core.Services.IoT
{
    public struct TemperatureValue
    {
        public TemperatureValue(float value) : this(value, default(TemperatureTypes))
        {
        }

        public TemperatureValue(float rawValue, TemperatureTypes unit)
        {
            RawValue = rawValue;
            Unit = unit;
        }

        public TemperatureTypes Unit { get; private set; }
        public float RawValue { get; private set; }

    }
}