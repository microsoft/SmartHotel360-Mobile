using CoreGraphics;
using CoreLocation;
using MapKit;
using SmartHotel.Clients.Core.Controls;
using SmartHotel.Clients.Core.Helpers;
using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace SmartHotel.Clients.iOS.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        private UIImage EventImage = UIImage.FromFile("pushpin_01.png");
        private UIImage RestaurantImage = UIImage.FromFile("pushpin_02.png");

        private List<CustomPin> _customPins;
        private List<MKAnnotationView> _tempAnnotations;
        private UIView _customPinView;
        private CustomMap _customMap;
        private bool _isDrawnDone;

        public CustomMapRenderer()
        {
            _tempAnnotations = new List<MKAnnotationView>();
            _customPins = new List<CustomPin>();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var iosMapView = (MKMapView)Control;
            _customMap = (CustomMap)sender;

            if (e.PropertyName.Equals("CustomPins") && !_isDrawnDone)
            {
                _customPins = _customMap.CustomPins.ToList();

                ClearPushPins(iosMapView);

                iosMapView.ZoomEnabled = true;
                iosMapView.GetViewForAnnotation = GetViewForAnnotation;
                iosMapView.DidSelectAnnotationView += OnDidSelectAnnotationView;
                iosMapView.DidDeselectAnnotationView += OnDidDeselectAnnotationView;

                AddPushPins(iosMapView, _customMap.CustomPins);
                PositionMap();

                _isDrawnDone = true;
            }
        }

        private MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
                return null;

            var anno = annotation as MKAnnotation;
            var customPin = GetCustomPin(anno);

            if (customPin == null)
            {
                throw new Exception("Custom pin not found!");
            }

            annotationView = mapView.DequeueReusableAnnotation(customPin.Id.ToString());

            if (annotationView == null)
            {
                annotationView = new CustomMKAnnotationView(annotation, customPin.Id)
                {
                    CalloutOffset = new CGPoint(0, 0)
                };

                switch (customPin.Type)
                {
                    case SuggestionType.Event:
                        annotationView.Image = EventImage;
                        break;
                    case SuggestionType.Restaurant:
                        annotationView.Image = RestaurantImage;
                        break;
                }

                ((CustomMKAnnotationView)annotationView).Id = customPin.Id;
            }

            annotationView.CanShowCallout = true;

            return annotationView;
        }

        private void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            var customView = e.View as CustomMKAnnotationView;
            _customPinView = new UIView();
            e.View.AddSubview(_customPinView);

            var anotation = customView.Annotation as MKAnnotation;
            var selectedPin = GetCustomPin(anotation);
            var pin = _customPins.FirstOrDefault((p => p.Id == selectedPin.Id));

            if (pin != null)
            {
                _customMap.SelectedPin = pin;
            }
        }

        private void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            if (!e.View.Selected)
            {
                _customPinView.RemoveFromSuperview();
                _customPinView.Dispose();
                _customPinView = null;
            }
        }

        private void AddPushPins(MKMapView mapView, IEnumerable<CustomPin> pins)
        {
            foreach (var formsPin in pins)
            {
                var annotation = new CustomMKAnnotation(new
                    CLLocationCoordinate2D
                {
                    Latitude = formsPin.Position.Latitude,
                    Longitude = formsPin.Position.Longitude
                },
                    formsPin.Label);

                mapView.AddAnnotation(annotation);

                _tempAnnotations.Add(GetViewForAnnotation(mapView, annotation));
            }
        }

        private CustomPin GetCustomPin(MKAnnotation annotation)
        {
            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);

            foreach (var pin in _customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }

            return null;
        }

        private void ClearPushPins(MKMapView mapView)
        {
            mapView.RemoveAnnotations(mapView.Annotations);
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

    public class CustomMKAnnotation : MKAnnotation
    {
        private CLLocationCoordinate2D _coord;
        private string _title;

        public override CLLocationCoordinate2D Coordinate
        {
            get { return _coord; }
        }

        public override string Title
        {
            get { return _title; }
        }

        public override void SetCoordinate(CLLocationCoordinate2D coord)
        {
            _coord = coord;
        }

        public CustomMKAnnotation(
            CLLocationCoordinate2D coord,
            string title)
        {
            _coord = coord;
            _title = title;
        }
    }

    public class CustomMKAnnotationView : MKAnnotationView
    {
        public int Id { get; set; }

        public CustomMKAnnotationView(IMKAnnotation annotation, int id)
            : base(annotation, id.ToString())
        {
        }
    }
}