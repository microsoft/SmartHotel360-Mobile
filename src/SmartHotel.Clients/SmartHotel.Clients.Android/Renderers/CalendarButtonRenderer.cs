using System.Collections.Generic;
using Android.Runtime;
using Xamarin.Forms.Platform.Android;
using SmartHotel.Clients.Core.Controls;
using Android.Graphics.Drawables;
using System.Threading.Tasks;
using Xamarin.Forms;
using SmartHotel.Clients.Droid.Renderers;
using Android.Graphics;

[assembly: ExportRenderer(typeof(CalendarButton), typeof(CalendarButtonRenderer))]
namespace SmartHotel.Clients.Droid.Renderers
{
    [Preserve(AllMembers = true)]
    public class CalendarButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.TextChanged += (sender, a) =>
            {
                var element = Element as CalendarButton;
                if (Control.Text == element.TextWithoutMeasure || (string.IsNullOrEmpty(Control.Text) && string.IsNullOrEmpty(element.TextWithoutMeasure))) return;
                Control.Text = element.TextWithoutMeasure;
            };
            Control.SetPadding(0, 0, 0, 0);
            Control.ViewTreeObserver.GlobalLayout += (sender, args) => ChangeBackgroundPattern();
        }

        protected override async void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var element = Element as CalendarButton;
            if (e.PropertyName == nameof(element.TextWithoutMeasure) || e.PropertyName == "Renderer")
            {
                Control.Text = element.TextWithoutMeasure;
            }

            if (e.PropertyName == nameof(Element.TextColor) || e.PropertyName == "Renderer")
            {
                Control.SetTextColor(Element.TextColor.ToAndroid());
            }

            if (e.PropertyName == nameof(Element.BorderWidth) || e.PropertyName == nameof(Element.BorderColor) || e.PropertyName == nameof(Element.BackgroundColor) || e.PropertyName == "Renderer")
            {
                if (element.BackgroundPattern == null)
                {
                    if (element.BackgroundImage == null)
                    {
                        var drawable = new GradientDrawable();
                        drawable.SetShape(ShapeType.Rectangle);
                        drawable.SetStroke((int)Element.BorderWidth, Element.BorderColor.ToAndroid());
                        drawable.SetColor(Element.BackgroundColor.ToAndroid());
                        Control.SetBackground(drawable);
                    }
                    else
                    {
                        await ChangeBackgroundImageAsync();
                    }
                }
                else
                {
                    ChangeBackgroundPattern();
                }
            }

            if (e.PropertyName == nameof(element.BackgroundPattern))
            {
                ChangeBackgroundPattern();
            }

            if (e.PropertyName == nameof(element.BackgroundImage))
            {
                if (element.BackgroundImage == null)
                {
                    var drawable = new GradientDrawable();
                    drawable.SetShape(ShapeType.Rectangle);
                    drawable.SetStroke((int)Element.BorderWidth, Element.BorderColor.ToAndroid());
                    drawable.SetColor(Element.BackgroundColor.ToAndroid());
                    Control.SetBackground(drawable);
                }
                else
                {
                    await ChangeBackgroundImageAsync();
                }
            }
        }

        protected async Task ChangeBackgroundImageAsync()
        {
            var element = Element as CalendarButton;
            if (element == null || element.BackgroundImage == null) return;

            var d = new List<Drawable>();
            var image = await GetBitmap(element.BackgroundImage);
            d.Add(new BitmapDrawable(image));
            var layer = new LayerDrawable(d.ToArray());
            layer.SetLayerInset(d.Count - 1, 0, 0, 0, 0);
            Control?.SetBackground(layer);
        }

        protected void ChangeBackgroundPattern()
        {
            var element = Element as CalendarButton;
            if (element == null || element.BackgroundPattern == null || Control.Width == 0) return;

            var d = new List<Drawable>();
            for (var i = 0; i < element.BackgroundPattern.Pattern.Count; i++)
            {
                d.Add(new ColorDrawable(element.BackgroundPattern.Pattern[i].Color.ToAndroid()));
            }
            var drawable = new GradientDrawable();
            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetStroke((int)Element.BorderWidth, Element.BorderColor.ToAndroid());
            drawable.SetColor(Android.Graphics.Color.Transparent);
            d.Add(drawable);
            var layer = new LayerDrawable(d.ToArray());
            for (var i = 0; i < element.BackgroundPattern.Pattern.Count; i++)
            {
                var l = (int)(Control.Width * element.BackgroundPattern.GetLeft(i));
                var t = (int)(Control.Height * element.BackgroundPattern.GetTop(i));
                var r = (int)(Control.Width * (1 - element.BackgroundPattern.Pattern[i].WidthPercent)) - l;
                var b = (int)(Control.Height * (1 - element.BackgroundPattern.Pattern[i].HightPercent)) - t;
                layer.SetLayerInset(i, l, t, r, b);
            }
            layer.SetLayerInset(d.Count - 1, 0, 0, 0, 0);
            Control.SetBackground(layer);
        }

        Task<Bitmap> GetBitmap(FileImageSource image)
        {
            var handler = new FileImageSourceHandler();
            return handler.LoadImageAsync(image, this.Control.Context);
        }
    }

    public static class Calendar
    {
        public static void Init()
        {
            var d = string.Empty;
        }
    }
}