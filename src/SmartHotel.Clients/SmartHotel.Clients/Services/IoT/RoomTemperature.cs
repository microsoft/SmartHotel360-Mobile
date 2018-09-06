namespace SmartHotel.Clients.Core.Services.IoT
{
    public class RoomTemperature
    {
        private string _temperatureSensorId;

        private readonly TemperatureValue _defaultMinimum = new TemperatureValue(60);
        private readonly TemperatureValue _defaultMaximum = new TemperatureValue(90);

        private readonly TemperatureValue _defaultValue = new TemperatureValue(71);

        public RoomTemperature()
        {
            Value = _defaultValue;
            Desired = _defaultValue;
        }

        public RoomTemperature(TemperatureValue actualValue) 
        {
            Value = actualValue;
            Desired = actualValue;
        }

        public RoomTemperature(string temperatureSensorId): this()
        {
            _temperatureSensorId = temperatureSensorId;
            
        }

        public TemperatureValue Value { get; private set; }
        public TemperatureValue Desired { get; private set; }

        public TemperatureValue Minimum => _defaultMinimum;
        public TemperatureValue Maximum => _defaultMaximum;
    }
}