namespace SmartHotel.Clients.Core.Services.IoT
{
    public static class TemperatureExtesions
    {
        public static TemperatureValue ToFahrenheit(this TemperatureValue temp)
        {
            if (temp.Unit == TemperatureTypes.Fahrenheit)
                return temp;

            var fahr = 9.0 / 5.0 * temp.Value + 32;
            return new TemperatureValue((float)fahr, TemperatureTypes.Fahrenheit);
        }

        public static TemperatureValue ToCelsius(this TemperatureValue temp)
        {
            if (temp.Unit == TemperatureTypes.Celsius)
                return temp;

            var celc = (temp.Value - 32) * 5.0 / 9.0;
            return new TemperatureValue((float)celc, TemperatureTypes.Celsius);
        }
    }
}