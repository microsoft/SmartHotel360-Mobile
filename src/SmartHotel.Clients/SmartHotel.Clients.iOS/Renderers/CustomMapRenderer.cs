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
        readonly UIImage eventImage = UIImage.FromFile("pushpin_01.png");
        readonly UIImage restaurantImage = UIImage.FromFile("pushpin_02.png");

        List<CustomPin> customPins;
        List<MKAnnotationView> tempAnnotations;
        UIView customPinView;
        CustomMap customMap;
        bool isDrawnDone;

        public CustomMapRenderer()
        {
            tempAnnotations = new List<MKAnnotationView>();
            customPins = new List<CustomPin>();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var iosMapView = (MKMapView)Control;
            customMap = (CustomMap)sender;

            if (e.PropertyName.Equals("CustomPins") && !isDrawnDone)
            {
                customPins = customMap.CustomPins.ToList();

                ClearPushPins(iosMapView);

                iosMapView.ZoomEnabled = true;
                iosMapView.GetViewForAnnotation = GetViewForAnnotation;
                iosMapView.DidSelectAnnotationView += OnDidSelectAnnotationView;
                iosMapView.DidDeselectAnnotationView += OnDidDeselectAnnotationView;

                AddPushPins(iosMapView, customMap.CustomPins);
                PositionMap();

                isDrawnDone = true;
            }
        }

        MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
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
                        annotationView.Image = eventImage;
                        break;
                    case SuggestionType.Restaurant:
                        annotationView.Image = restaurantImage;
                        break;
                }

                ((CustomMKAnnotationView)annotationView).Id = customPin.Id;
            }

            annotationView.CanShowCallout = true;

            return annotationView;
        }

        void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            var customView = e.View as CustomMKAnnotationView;
            customPinView = new UIView();
            e.View.AddSubview(customPinView);

            var anotation = customView.Annotation as MKAnnotation;
            var selectedPin = GetCustomPin(anotation);
            var pin = customPins.FirstOrDefault((p => p.Id == selectedPin.Id));

            if (pin != null)
            {
                customMap.SelectedPin = pin;
            }
        }

        void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            if (!e.View.Selected)
            {
                customPinView.RemoveFromSuperview();
                customPinView.Dispose();
                customPinView = null;
            }
        }

        void AddPushPins(MKMapView mapView, IEnumerable<CustomPin> pins)
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

                tempAnnotations.Add(GetViewForAnnotation(mapView, annotation));
            }
        }

        CustomPin GetCustomPin(MKAnnotation annotation)
        {
            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);

            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }

            return null;
        }

        void ClearPushPins(MKMapView mapView) => mapView.RemoveAnnotations(mapView.Annotations);

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

    public class CustomMKAnnotation : MKAnnotation
    {
        CLLocationCoordinate2D coord;
        readonly string title;

        public override CLLocationCoordinate2D Coordinate => coord;

        public override string Title => title;

        public override void SetCoordinate(CLLocationCoordinate2D coord) => this.coord = coord;

        public CustomMKAnnotation(
            CLLocationCoordinate2D coord,
            string title)
        {
            this.coord = coord;
            this.title = title;
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