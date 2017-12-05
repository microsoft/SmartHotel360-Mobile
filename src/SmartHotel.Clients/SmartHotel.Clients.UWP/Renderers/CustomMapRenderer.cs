using SmartHotel.Clients.Core.Controls;
using SmartHotel.Clients.Core.Helpers;
using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.UWP.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace SmartHotel.Clients.UWP.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        private RandomAccessStreamReference EventResource = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/pushpin_01.png"));
        private RandomAccessStreamReference RestaurantResource = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/pushpin_02.png"));

        private CustomMap _customMap;
        private List<CustomPin> _customPins;
        private List<CustomMapIcon> _tempMapIcons;

        public CustomMapRenderer()
        {
            _customPins = new List<CustomPin>();
            _tempMapIcons = new List<CustomMapIcon>();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var windowsMapView = (MapControl)Control;
            _customMap = (CustomMap)sender;

            if (e.PropertyName.Equals("CustomPins"))
            {
                _customPins = _customMap.CustomPins.ToList();
                ClearPushPins(windowsMapView);

                AddPushPins(windowsMapView, _customMap.CustomPins);
                PositionMap();
            }
        }

        private void ClearPushPins(MapControl mapControl)
        {
            mapControl.MapElements.Clear();
        }

        private void AddPushPins(MapControl mapControl, IEnumerable<CustomPin> pins)
        {
            if (_tempMapIcons != null)
            {
                _tempMapIcons.Clear();
            }

            foreach (var pin in pins)
            {
                var snPosition = new BasicGeoposition { Latitude = pin.Position.Latitude, Longitude = pin.Position.Longitude };
                var snPoint = new Geopoint(snPosition);

                var mapIcon = new CustomMapIcon
                {
                    Label = pin.Label,
                    MapIcon = new MapIcon
                    {
                        MapTabIndex = pin.Id,
                        CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible,
                        Location = snPoint,
                        NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 1.0),
                        ZIndex = 0
                    }
                };

                switch (pin.Type)
                {
                    case SuggestionType.Event:
                        mapIcon.MapIcon.Image = EventResource;
                        break;
                    case SuggestionType.Restaurant:
                        mapIcon.MapIcon.Image = RestaurantResource;
                        break;
                    default:
                        mapIcon.MapIcon.Image = EventResource;
                        break;
                }

                mapControl.MapElements.Add(mapIcon.MapIcon);
                _tempMapIcons.Add(mapIcon);
            }
        }

        private void PositionMap()
        {
            var myMap = this.Element as CustomMap;
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

    public class CustomMapIcon
    {
        public string Label { get; set; }
        public MapIcon MapIcon { get; set; }
    }
}