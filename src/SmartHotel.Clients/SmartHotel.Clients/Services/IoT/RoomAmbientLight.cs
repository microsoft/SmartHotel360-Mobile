namespace SmartHotel.Clients.Core.Services.IoT
{
    public class RoomAmbientLight : RoomTemperatureBase
    {
        private string _lightSensorId;

        private static readonly TemperatureValue DefaultMinimum = new TemperatureValue(2800, TemperatureTypes.Kelvin);
        private static readonly TemperatureValue DefaultMaximum = new TemperatureValue(4600, TemperatureTypes.Kelvin);
        private static readonly TemperatureValue DefaultValue = new TemperatureValue(4500, TemperatureTypes.Kelvin);

        private RoomAmbientLight(TemperatureValue desired) : base(DefaultValue, DefaultMinimum, DefaultMaximum)
        {
            Desired = desired;
        }

        public RoomAmbientLight() : this(DefaultValue)
        {
        }

        public RoomAmbientLight(string temperatureSensorId, TemperatureValue desired) : this(temperatureSensorId, DefaultValue, desired)
        {
            _lightSensorId = temperatureSensorId;
        }

        public RoomAmbientLight(string temperatureSensorId, TemperatureValue value, TemperatureValue desired) : base(value, desired, DefaultMinimum, DefaultMaximum)
        {
            _lightSensorId = temperatureSensorId;
        }


    }
}