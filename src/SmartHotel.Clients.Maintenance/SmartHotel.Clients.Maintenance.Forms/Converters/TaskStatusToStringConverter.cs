using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartHotel.Clients.Maintenance.Forms.Converters
{
    public class TaskStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                var resolved = (bool)value;

                if (!resolved)
                    return "Pending";
                else
                    return "Resolved";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}