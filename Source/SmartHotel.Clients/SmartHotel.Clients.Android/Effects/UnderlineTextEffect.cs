using Android.Widget;
using SmartHotel.Clients.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(UnderlineTextEffect), "UnderlineTextEffect")]
namespace SmartHotel.Clients.Droid.Effects
{
    public class UnderlineTextEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Control is TextView label)
            {
                label.PaintFlags |= Android.Graphics.PaintFlags.UnderlineText;
            }
        }

        protected override void OnDetached()
        {
        }
    }
}