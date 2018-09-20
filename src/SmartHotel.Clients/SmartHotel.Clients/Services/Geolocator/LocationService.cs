using Xamarin.Essentials;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SmartHotel.Clients.Core.Extensions;

namespace SmartHotel.Clients.Core.Services.Geolocator
{
    public class LocationService : ILocationService
    {
        readonly TimeSpan positionReadTimeout = TimeSpan.FromSeconds(5);

        public async Task<Location> GetPositionAsync()
        {
            Location position = null;
            try
            {

                position = await Geolocation.GetLastKnownLocationAsync();

                if (position != null)
                {
                    // Got a cahched position, so let's use it.
                    return position;
                }
                                
                position = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location: " + ex);
            }

            var defaultLocation = AppSettings.DefaultFallbackMapsLocation.ParseLocation();

            return defaultLocation;
        }
    }
}