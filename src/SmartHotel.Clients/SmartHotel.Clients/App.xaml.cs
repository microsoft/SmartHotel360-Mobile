using Microsoft.Identity.Client;
using SmartHotel.Clients.Core;
using SmartHotel.Clients.Core.Services.Navigation;
using SmartHotel.Clients.Core.ViewModels;
using SmartHotel.Clients.Core.ViewModels.Base;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SmartHotel.Clients
{
    public partial class App : Application
    {
        public static PublicClientApplication AuthenticationClient { get; set; }

        static App()
        {
            BuildDependencies();
        }
        public App()
        {
            AuthenticationClient =
                new PublicClientApplication($"{AppSettings.B2cAuthority}{AppSettings.B2cTenant}", AppSettings.B2cClientId);

            InitializeComponent();
    
            InitNavigation();
        }

        public static void BuildDependencies()
        {
            // Do you want to use fake services that DO NOT require real backend or internet connection?
            // Set to true the value to use fake services, false if you want to use Azure Services.
            AppSettings.UseFakes = true;

            Locator.Instance.Build();
        }

        private Task InitNavigation()
        {
            var navigationService = Locator.Instance.Resolve<INavigationService>();
            return navigationService.NavigateToAsync<ExtendedSplashViewModel>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start($"ios={AppSettings.AppCenterAnalyticsIos};" +
                $"uwp={AppSettings.AppCenterAnalyticsWindows};" +
                $"android={AppSettings.AppCenterAnalyticsAndroid}",
                typeof(Analytics), typeof(Crashes), typeof(Distribute));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
