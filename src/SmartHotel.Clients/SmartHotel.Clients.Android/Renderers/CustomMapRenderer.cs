using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using SmartHotel.Clients.Core.Controls;
using SmartHotel.Clients.Core.Helpers;
using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Droid.Renderers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace SmartHotel.Clients.Droid.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        const int eventResource = Resource.Drawable.pushpin_01;
        const int restaurantResource = Resource.Drawable.pushpin_02;

        BitmapDescriptor pinIcon;
        List<CustomMarkerOptions> tempMarkers;
        bool isDrawnDone;

        public CustomMapRenderer(Context context) : base(context)
        {
            tempMarkers = new List<CustomMarkerOptions>();
            pinIcon = BitmapDescriptorFactory.FromResource(eventResource);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var androidMapView = (MapView)Control;
            var formsMap = (CustomMap)sender;

            if(e.PropertyName.Equals("CustomPins") && !isDrawnDone)
            {
                ClearPushPins(androidMapView);

                NativeMap.MyLocationEnabled = formsMap.IsShowingUser;

                AddPushPins(androidMapView, formsMap.CustomPins);

                PositionMap();

                isDrawnDone = true;
            }
        } 

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (changed)
            {
                isDrawnDone = false;
            }
        }

        void ClearPushPins(MapView mapView) => NativeMap.Clear();

        void AddPushPins(MapView mapView, IEnumerable<CustomPin> pins)
        {
            foreach (var formsPin in pins)
            {
                var markerWithIcon = new MarkerOptions();

                markerWithIcon.SetPosition(new LatLng(formsPin.Position.Latitude, formsPin.Position.Longitude));
                markerWithIcon.SetTitle(formsPin.Label);
                markerWithIcon.SetSnippet(formsPin.Address);

                switch (formsPin.Type)
                {
                    case SuggestionType.Event:
                        pinIcon = BitmapDescriptorFactory.FromResource(eventResource);
                        markerWithIcon.SetIcon(pinIcon);
                        break;
                    case SuggestionType.Restaurant:
                        pinIcon = BitmapDescriptorFactory.FromResource(restaurantResource);
                        markerWithIcon.SetIcon(pinIcon);
                        break;
                    default:
                        markerWithIcon.SetIcon(BitmapDescriptorFactory.DefaultMarker());
                        break;
                }

                NativeMap.AddMarker(markerWithIcon);

                tempMarkers.Add(new CustomMarkerOptions
                {
                    Id = formsPin.Id,
                    MarkerOptions = markerWithIcon
                });
            }
        }

        void PositionMap()
        {
            var myMap = Element as CustomMap;
            var formsPins = myMap.CustomPins;

            if (formsPins == null || formsPins.Count() == 0)
            {
                return;
            }

            var centerPosition = new Position(formsPins.Average(x => x.Position.Latitude), formsPins.Average(x => x.Position.Longitude));

            var minLongitude = formsPins.Min(x => x.Position.Longitude);
            var minLatitude = formsPins.Min(x => x.Position.Latitude);

            var maxLongitude = formsPins.Max(x => x.Position.Longitude);
            var maxLatitude = formsPins.Max(x => x.Position.Latitude);

            var distance = MapHelper.CalculateDistance(minLatitude, minLongitude,
                maxLatitude, maxLongitude, 'M') / 2;

            myMap.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(distance)));
        }
    }

    public class CustomMarkerOptions
    {
        public int Id { get; set; }
        public MarkerOptions MarkerOptions { get; set; }
    }
}