using CoreAnimation;
using CoreGraphics;
using SmartHotel.Clients.Core.Controls;
using SmartHotel.Clients.iOS.Renderers;
using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace SmartHotel.Clients.iOS.Renderers
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        public ExtendedEntry ExtendedEntryElement => Element as ExtendedEntry;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    Control.BorderStyle = UIKit.UITextBorderStyle.None;
                }

                UpdateLineColor();
                UpdateCursorColor();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals(nameof(ExtendedEntry.LineColorToApply)))
            {
                UpdateLineColor();
            }
            else if (e.PropertyName.Equals(Entry.TextColorProperty.PropertyName))
            {
                UpdateCursorColor();
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            LineLayer lineLayer = GetOrAddLineLayer();
            lineLayer.Frame = new CGRect(0, Frame.Size.Height - LineLayer.LineHeight, Control.Frame.Size.Width, LineLayer.LineHeight);
        }

        private void UpdateLineColor()
        {
            LineLayer lineLayer = GetOrAddLineLayer();
            lineLayer.BorderColor = ExtendedEntryElement.LineColorToApply.ToCGColor();
        }

        private LineLayer GetOrAddLineLayer()
        {
            var lineLayer = Control.Layer.Sublayers?.OfType<LineLayer>().FirstOrDefault();
            
            if (lineLayer == null)
            {
                lineLayer = new LineLayer();

                Control.Layer.AddSublayer(lineLayer);
                Control.Layer.MasksToBounds = true;
            }

            return lineLayer;
        }

        private void UpdateCursorColor()
        {
            Control.TintColor = Element.TextColor.ToUIColor();
        }

        class LineLayer : CALayer
        {
            public static nfloat LineHeight = 2f;

            public LineLayer()
            {
                BorderWidth = LineHeight;
            }
        }
    }
}