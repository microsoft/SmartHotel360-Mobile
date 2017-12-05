using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartHotel.Clients.Maintenance.Forms.Converters
{
    public class TaskTypeToSubtitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var taskType = (int)value;

                switch (taskType)
                {
                    case 1:
                        return "Air Conditioner";
                    case 2:
                        return "Clean Room";
                    case 3:
                        return "New Guest";
                    case 4:
                        return "Room Service";
                    case 5:
                        return "Change Towels";
                    default:
                        return "Room Service";
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}