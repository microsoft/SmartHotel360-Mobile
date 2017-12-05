using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using SmartHotel.Clients.Core.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Location
{
    public class LocationService : ILocationService
    {
        private readonly TimeSpan PositionReadTimeout = TimeSpan.FromSeconds(5);

        public async Task<GeoLocation> GetPositionAsync()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                var position = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromMilliseconds(
                    (int)PositionReadTimeout.TotalMilliseconds));

                var geolocation = new GeoLocation
                {
                    Latitude = position.Latitude,
                    Longitude = position.Longitude,
                };

                return geolocation;
            }
            catch (GeolocationException geoEx)
            {
                Debug.WriteLine(geoEx);
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine(ex);
            }

            var defaultLocation = GeoLocation.Parse(AppSettings.DefaultFallbackMapsLocation);

            return defaultLocation;
        }
    }
}