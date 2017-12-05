using SmartHotel.Clients.Core.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms.Maps;

namespace SmartHotel.Clients.Core.Helpers
{
    public static class MapHelper
    {
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2)) + Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) * Math.Cos(Deg2Rad(theta));
            dist = Math.Acos(dist);
            dist = Rad2Deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        internal static void CenterMapInDefaultLocation(Map map)
        {
            try
            {
                var location = GeoLocation.Parse(AppSettings.FallbackMapsLocation);
                var initialPosition = new Position(
                      location.Latitude,
                      location.Longitude);

                var mapSpan = MapSpan.FromCenterAndRadius(
                    initialPosition,
                    Distance.FromMiles(1.0));

                map.MoveToRegion(mapSpan);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"[MapHelper] Error: {ex}");
            }
        }

        private static double Deg2Rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private static double Rad2Deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}