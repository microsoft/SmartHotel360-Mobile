using Xamarin.Forms;
using SmartHotel.Clients.Core.Helpers;

namespace SmartHotel.Clients.Core.Views
{
    public partial class BookingHotelView : ContentPage
    {
        private const int ParallaxSpeed = 4;

        private double _lastScroll;

        public BookingHotelView()
        {
            if (Device.RuntimePlatform != Device.iOS)
            {
                NavigationPage.SetHasNavigationBar(this, false);
            }

            NavigationPage.SetBackButtonTitle(this, string.Empty);

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnDisappearing();

            StatusBarHelper.Instance.MakeTranslucentStatusBar(false);

            ParallaxScroll.ParallaxView = HeaderView;
            ParallaxScroll.Scrolled += OnParallaxScrollScrolled;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ParallaxScroll.Scrolled -= OnParallaxScrollScrolled;
        }

        private void OnParallaxScrollScrolled(object sender, ScrolledEventArgs e)
        {
            double translation = 0;

            if (_lastScroll == 0)
            {
                _lastScroll = e.ScrollY;
            }

            if (_lastScroll < e.ScrollY)
            {
                translation = 0 - ((e.ScrollY / ParallaxSpeed));

                if (translation > 0) translation = 0;
            }
            else
            {
                translation = 0 + ((e.ScrollY / ParallaxSpeed));

                if (translation > 0) translation = 0;
            }

            SubHeaderView.FadeTo(translation < -40 ? 0 : 1);
   
            _lastScroll = e.ScrollY;
        }
    }
}