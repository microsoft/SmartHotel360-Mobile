using SmartHotel.Clients.Core.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Converters
{
    public class SuggestionTypeToColorConverter : IValueConverter
    {
        readonly Color restaurantColor = Color.FromHex("#BD4B14");
        readonly Color eventColor = Color.FromHex("#348E94");
        readonly Color noColor = Color.FromHex("#FFFFFF");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SuggestionType suggestionType)
            {
                switch (suggestionType)
                {
                    case SuggestionType.Event:
                        return eventColor;
                    case SuggestionType.Restaurant:
                        return restaurantColor;
                }
            }


            return noColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}