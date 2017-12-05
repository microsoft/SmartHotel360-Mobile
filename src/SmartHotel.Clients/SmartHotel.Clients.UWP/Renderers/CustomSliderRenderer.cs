using SmartHotel.Clients.UWP.Renderers;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Slider), typeof(CustomSliderRenderer))]
namespace SmartHotel.Clients.UWP.Renderers
{
    public class CustomSliderRenderer : SliderRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Slider> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.IsThumbToolTipEnabled = false;
            }
        }
    }
}