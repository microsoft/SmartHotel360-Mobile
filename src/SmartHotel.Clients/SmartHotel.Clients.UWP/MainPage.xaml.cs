using FFImageLoading.Forms.WinUWP;
using SmartHotel.Clients.Core;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace SmartHotel.Clients.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            CachedImageRenderer.Init();
            Renderers.Calendar.Init();
            Xamarin.FormsMaps.Init(AppSettings.BingMapsApiKey);
            LoadApplication(new Clients.App());
            NativeCustomize();
        }

        private void NativeCustomize()
        {
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 500));

            // PC Customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.BackgroundColor = (Color)App.Current.Resources["NativeAccentColor"];
                    titleBar.ButtonBackgroundColor = (Color)App.Current.Resources["NativeAccentColor"];
                }
            }

            // Mobile Customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = (Color)App.Current.Resources["NativeAccentColor"];
                }
            }

            // Launch in Window Mode
            var currentView = ApplicationView.GetForCurrentView();
            if (currentView.IsFullScreenMode)
            {
                currentView.ExitFullScreenMode();
            }
        }
    }
}