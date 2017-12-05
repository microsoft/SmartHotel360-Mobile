using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartHotel.Clients.Maintenance.Forms.Converters
{
    public class TaskTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var taskType = (int)value;

                switch (taskType)
                {
                    case 1:
                        return "ic_air_conditioner.png";
                    case 2:
                        return "ic_clean_room.png";
                    case 3:
                        return "ic_new_guest.png";
                    case 4:
                        return "ic_room_service.png";
                    case 5:
                        return "ic_towel.png";
                    default:
                        return "ic_room_service.png";

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