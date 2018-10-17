using System;
using System.Globalization;
using Xamarin.Essentials;

namespace SmartHotel.Clients.Core.Extensions
{
    public static class LocationExtensions
    {
        public static Location ParseLocation(this string location)
        {
            var result = new Location();
            try
            {
                var locationSetting = location;
                var locationParts = locationSetting.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                result.Latitude = double.Parse(locationParts[0], CultureInfo.InvariantCulture);
                result.Longitude = double.Parse(locationParts[1], CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error parsing location: {ex}");
            }

            return result;
        }
    }
}
