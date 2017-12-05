using SmartHotel.Clients.Core.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Converters
{
    public class SuggestionTypeToColorConverter : IValueConverter
    {
        private Color RestaurantColor = Color.FromHex("#BD4B14");
        private Color EventColor = Color.FromHex("#348E94");
        private Color NoColor = Color.FromHex("#FFFFFF");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is SuggestionType)
            {
                var suggestionType = (SuggestionType)value;

                switch(suggestionType)
                {
                    case SuggestionType.Event:
                        return EventColor;
                    case SuggestionType.Restaurant:
                        return RestaurantColor;
                }
            }


            return NoColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}