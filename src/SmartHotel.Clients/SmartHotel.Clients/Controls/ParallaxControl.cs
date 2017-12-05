using Xamarin.Forms;

// https://github.com/xamarinhq/app-evolve 
namespace SmartHotel.Clients.Core.Controls
{
    public class ParallaxControl : ScrollView
    {
        public ParallaxControl()
        {
            Scrolled += (sender, e) => Parallax();
        }
        public static readonly BindableProperty ParallaxViewProperty =
            BindableProperty.Create(nameof(ParallaxControl), typeof(View), typeof(ParallaxControl), null);

        public View ParallaxView
        {
            get { return (View)GetValue(ParallaxViewProperty); }
            set { SetValue(ParallaxViewProperty, value); }
        }

        double height;
        public void Parallax()
        {
            if (ParallaxView == null 
                || Device.RuntimePlatform == "Windows" 
                || Device.RuntimePlatform == "WinPhone")
                return;

            if (height <= 0)
                height = ParallaxView.Height;

            var y = -(int)((float)ScrollY / 2.5f);

            if (y < 0)
            {
                ParallaxView.Scale = 1;
                ParallaxView.TranslationY = y;
            }
            else if (Device.RuntimePlatform == "iOS")
            {
                double newHeight = height + (ScrollY * -1);
                ParallaxView.Scale = newHeight / height;
                ParallaxView.TranslationY = -(ScrollY / 2);
            }
            else
            {
                ParallaxView.Scale = 1;
                ParallaxView.TranslationY = 0;
            }
        }
    }
}