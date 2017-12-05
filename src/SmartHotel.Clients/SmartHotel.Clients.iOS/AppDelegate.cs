using CarouselView.FormsPlugin.iOS;
using FFImageLoading.Forms.Touch;
using Foundation;
using Microcharts.Forms;
using Microsoft.Identity.Client;
using SmartHotel.Clients.Core.Services.Authentication;
using SmartHotel.Clients.Core.ViewModels.Base;
using SmartHotel.Clients.iOS.Services;
using UIKit;
using Xamarin.Forms;

namespace SmartHotel.Clients.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {        
            // Newer version of Xamarin Studio and Visual Studio provide the
            // ENABLE_TEST_CLOUD compiler directive in the Debug configuration,
            // but not the Release configuration.

#if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();

            // Mapping StyleId to iOS Labels
            Forms.ViewInitialized += (object sender, ViewInitializedEventArgs e) =>
            {
                if (null != e.View.StyleId)
                {
                    e.NativeView.AccessibilityIdentifier = e.View.StyleId;
                }
            };
#endif
            Forms.Init();
            CachedImageRenderer.Init();
            CarouselViewRenderer.Init();
            Renderers.Calendar.Init();
            Xamarin.FormsMaps.Init();
            InitChartView();
            InitXamanimation();

            RegisterPlatformDependencies();
            LoadApplication(new App());

            base.FinishedLaunching(app, options);

            UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            UINavigationBar.Appearance.ShadowImage = new UIImage();
            UINavigationBar.Appearance.BackgroundColor = UIColor.Clear;
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.BarTintColor = UIColor.Clear;
            UINavigationBar.Appearance.Translucent = true;

            // Initialize B2C client
            App.AuthenticationClient.PlatformParameters = new PlatformParameters(UIApplication.SharedApplication.KeyWindow.RootViewController);

            return true;
        }

        private void RegisterPlatformDependencies()
        {
            Locator.Instance.Register<IBrowserCookiesService, BrowserCookiesService>();
        }

        private static void InitChartView()
        {
            var t1 = typeof(ChartView);
        }

        private static void InitXamanimation()
        {
            var t2 = typeof(Xamanimation.AnimationBase);
        }
    }
}
