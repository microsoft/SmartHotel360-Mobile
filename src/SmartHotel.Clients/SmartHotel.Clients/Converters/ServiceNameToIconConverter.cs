using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Converters
{
    public class ServiceNameToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!string.IsNullOrEmpty(value.ToString()))
            {
                var icon = value.ToString();

                switch(icon)
                {
                    case "Airport shuttle":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_airport_shutle.png" : "ic_airport_shutle";
                    case "Bath":
                    case "Shared bath":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_bath.png" : "ic_bath";
                    case "Breakfast":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_breakfast.png" : "ic_breakfast";
                    case "Elevator":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_elevator.png" : "ic_elevator";
                    case "Free Wi-fi":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_wifi.png" : "ic_wifi";
                    case "Green":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_green.png" : "ic_green";
                    case "Air conditioning":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_air_conditioning.png" : "ic_air_conditioning";
                    case "Fitness centre":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_fitness_centre.png" : "ic_fitness_centre";
                    case "Dryer":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_dryer.png" : "ic_dryer";
                    case "Indoor fireplace":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_indoor_fireplace.png" : "ic_indoor_fireplace";
                    case "Gym":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_gym.png" : "ic_gym";
                    case "Hot tub":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_hot_tub.png" : "ic_hot_tub";
                    case "Laptop workspace":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_table.png" : "ic_table";
                    case "Parking":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_parking.png" : "ic_parking";
                    case "Swimming pool":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_pool.png" : "ic_pool";
                    case "TV":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_tv.png" : "ic_tv";
                    case "Wheelchair accessible":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_wheelchair_accessible.png" : "ic_wheelchair_accessible";
                    case "Work Table":
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_table.png" : "ic_table";
                    default:
                        return Device.RuntimePlatform == Device.UWP ? "Assets/ic_green.png" : "ic_green";
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}