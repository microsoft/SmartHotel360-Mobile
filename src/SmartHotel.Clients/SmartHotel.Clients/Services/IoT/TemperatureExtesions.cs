namespace SmartHotel.Clients.Core.Services.IoT
{
    public static class TemperatureExtesions
    {
        public static SensorValue ToFahrenheit(this SensorValue temp)
        {
            if (temp.Unit == SensorType.Fahrenheit)
                return temp;

            var fahr = 9.0 / 5.0 * temp.RawValue + 32;
            return new SensorValue((float)fahr, SensorType.Fahrenheit);
        }

        public static SensorValue ToCelsius(this SensorValue temp)
        {
            if (temp.Unit == SensorType.Celsius)
                return temp;

            var celc = (temp.RawValue - 32) * 5.0 / 9.0;
            return new SensorValue((float)celc, SensorType.Celsius);
        }
    }
}