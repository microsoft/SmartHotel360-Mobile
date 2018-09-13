using SmartHotel.Clients.Core.Extensions;
using SmartHotel.Clients.Core.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms.Maps;

namespace SmartHotel.Clients.Core.Helpers
{
    public static class MapHelper
    {
        internal static void CenterMapInDefaultLocation(Map map)
        {
            try
            {
                var location = AppSettings.FallbackMapsLocation.ParseLocation();
                var initialPosition = new Position(
                      location.Latitude,
                      location.Longitude);

                var mapSpan = MapSpan.FromCenterAndRadius(
                    initialPosition,
                    Distance.FromMiles(1.0));

                map.MoveToRegion(mapSpan);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[MapHelper] Error: {ex}");
            }
        }
    }
}