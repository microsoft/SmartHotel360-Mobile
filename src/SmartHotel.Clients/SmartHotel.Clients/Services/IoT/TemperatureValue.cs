namespace SmartHotel.Clients.Core.Services.IoT
{
    public struct TemperatureValue
    {
        public TemperatureValue(float value) : this(value, default(TemperatureTypes))
        {
        }

        public TemperatureValue(float value, TemperatureTypes unit)
        {
            Value = value;
            Unit = unit;
        }

        public TemperatureTypes Unit { get; private set; }
        public float Value { get; private set; }

    }
}