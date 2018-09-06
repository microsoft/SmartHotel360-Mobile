namespace SmartHotel.Clients.Core.Services.IoT
{
    public class RoomAmbientLight : RoomTemperatureBase
    {
        private string _lightSensorId;

        private static readonly TemperatureValue DefaultMinimum = new TemperatureValue(2800, TemperatureTypes.Kelvin);
        private static readonly TemperatureValue DefaultMaximum = new TemperatureValue(4600, TemperatureTypes.Kelvin);
        private static readonly TemperatureValue DefaultValue = new TemperatureValue(4500);

        public RoomAmbientLight(): base(DefaultValue, DefaultMinimum, DefaultMaximum)
        {

        }

        public RoomAmbientLight(string lightSensorId) : this()
        {
            _lightSensorId = lightSensorId;
        }
    }
}