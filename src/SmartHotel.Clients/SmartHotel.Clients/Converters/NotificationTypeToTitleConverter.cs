using SmartHotel.Clients.Core.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Converters
{
    public class NotificationTypeToTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is NotificationType)
            {
                var notificationType = (NotificationType)value;

                switch(notificationType)
                {
                    case NotificationType.BeGreen:
                        return "Be Green";
                    case NotificationType.Hotel:
                        return "Hotel";
                    case NotificationType.Room:
                        return "Room";
                    case NotificationType.Other:
                        return "Other";
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
