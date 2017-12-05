using SmartHotel.Clients.Core.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Converters
{
    public class NotificationTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is NotificationType)
            {
                var notificationType = (NotificationType)value;

                switch(notificationType)
                {
                    case NotificationType.BeGreen:
                        return Device.RuntimePlatform == Device.UWP ? string.Format("Assets/ic_be_green{0}.png", parameter ?? string.Empty) : string.Format("ic_be_green{0}", parameter ?? string.Empty);
                    case NotificationType.Hotel:
                        return Device.RuntimePlatform == Device.UWP ? string.Format("Assets/ic_hotel{0}.png", parameter ?? string.Empty) : string.Format("ic_hotel{0}", parameter ?? string.Empty);
                    case NotificationType.Room:
                        return Device.RuntimePlatform == Device.UWP ? string.Format("Assets/ic_room{0}.png", parameter ?? string.Empty) : string.Format("ic_room{0}", parameter ?? string.Empty);
                    case NotificationType.Other:
                        return Device.RuntimePlatform == Device.UWP ? string.Format("Assets/ic_others{0}.png", parameter ?? string.Empty) : string.Format("ic_others{0}", parameter ?? string.Empty);
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