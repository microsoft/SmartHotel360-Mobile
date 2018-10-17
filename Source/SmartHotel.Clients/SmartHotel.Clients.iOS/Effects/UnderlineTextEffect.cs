using Foundation;
using SmartHotel.Clients.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(UnderlineTextEffect), "UnderlineTextEffect")]
namespace SmartHotel.Clients.iOS.Effects
{
    public class UnderlineTextEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var element = Element as Label;

            if (Control is UILabel label && element != null)
            {
                var attributes = new UIStringAttributes { UnderlineStyle = NSUnderlineStyle.Single };
                var attrString = new NSAttributedString(element.Text, attributes);
                label.AttributedText = attrString;
            }
        }

        protected override void OnDetached()
        {
        }
    }
}