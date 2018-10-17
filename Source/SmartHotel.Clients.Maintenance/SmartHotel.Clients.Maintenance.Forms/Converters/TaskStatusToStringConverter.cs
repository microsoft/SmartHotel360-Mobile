using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartHotel.Clients.Maintenance.Forms.Converters
{
    public class TaskStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is bool resolved ? !resolved ? "Pending" : "Resolved" : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}