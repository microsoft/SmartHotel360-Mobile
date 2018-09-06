namespace SmartHotel.Clients.Core.Services.IoT
{
    public class RoomAmbientLight : RoomTemperatureBase
    {
        private string _lightSensorId;

        private static readonly TemperatureValue DefaultMinimum = new TemperatureValue(2800, TemperatureTypes.Kelvin);
        private static readonly TemperatureValue DefaultMaximum = new TemperatureValue(4600, TemperatureTypes.Kelvin);
        private static readonly TemperatureValue DefaultValue = new TemperatureValue(4500, TemperatureTypes.Kelvin);

        private static readonly TemperatureValue FakeDesiredValue = new TemperatureValue(3000, TemperatureTypes.Kelvin);

        private RoomAmbientLight(TemperatureValue desired): base(DefaultValue, DefaultMinimum, DefaultMaximum)
        {
            Desired = desired;
        }

        public static RoomAmbientLight CreateFake()
        {
            return new RoomAmbientLight(FakeDesiredValue);
        }

        public RoomAmbientLight(string lightSensorId) : this(lightSensorId, DefaultValue)
        {
        }

        public RoomAmbientLight(string lightSensorId, TemperatureValue value) : base(value, DefaultMinimum, DefaultMaximum)
        {
            _lightSensorId = lightSensorId;
        }


    }
}