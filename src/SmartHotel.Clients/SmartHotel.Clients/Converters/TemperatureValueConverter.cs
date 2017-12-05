using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Converters
{
    public class TemperatureValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double))
            {
                throw new InvalidOperationException("The target must be a double");
            }

            var lightValue = (double)value;

            return $"{lightValue.ToString("N0")}°F";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
