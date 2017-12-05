using SmartHotel.Clients.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Slider), typeof(CustomSliderRenderer))]
namespace SmartHotel.Clients.Droid.Renderers
{
    public class CustomSliderRenderer : SliderRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);

            Control?.SetPadding(0, 0, 0, 0);
        }
    }
}