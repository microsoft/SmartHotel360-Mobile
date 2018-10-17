using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartHotel.Clients.Maintenance.Forms.Converters
{
    public class TaskStatusToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is bool resolved ? !resolved ? "pending-status.png" : "resolved-status.png" : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}