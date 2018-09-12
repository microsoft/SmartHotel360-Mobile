namespace SmartHotel.Clients.Core.Services.IoT
{
    public static class TemperatureExtesions
    {
        public static SensorValue ToFahrenheit(this SensorValue temp)
        {
            if (temp.Unit == SensorTypes.Fahrenheit)
                return temp;

            var fahr = 9.0 / 5.0 * temp.RawValue + 32;
            return new SensorValue((float)fahr, SensorTypes.Fahrenheit);
        }

        public static SensorValue ToCelsius(this SensorValue temp)
        {
            if (temp.Unit == SensorTypes.Celsius)
                return temp;

            var celc = (temp.RawValue - 32) * 5.0 / 9.0;
            return new SensorValue((float)celc, SensorTypes.Celsius);
        }
    }
}