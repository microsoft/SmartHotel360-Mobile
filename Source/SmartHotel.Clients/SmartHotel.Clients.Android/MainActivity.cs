using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Microsoft.Identity.Client;
using Rg.Plugins.Popup;
using Rg.Plugins.Popup.Services;
using SmartHotel.Clients.Core;
using SmartHotel.Clients.Core.Helpers;
using SmartHotel.Clients.Core.Services.Authentication;
using SmartHotel.Clients.Core.ViewModels.Base;
using SmartHotel.Clients.Droid.Services.Authentication;
using SmartHotel.Clients.Droid.Services.CardEmulation;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CarouselViewRenderer = CarouselView.FormsPlugin.Android.CarouselViewRenderer;

namespace SmartHotel.Clients.Droid
{
    [Activity(
        Label = "SmartHotel", 
        Icon = "@drawable/icon", 
        Theme = "@style/MainTheme", 
        MainLauncher = false,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            CarouselViewRenderer.Init();
            Renderers.Calendar.Init();
            Xamarin.FormsMaps.Init(this, bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(false);
            Popup.Init(this, bundle);
            Acr.UserDialogs.UserDialogs.Init(this);

            InitMessageCenterSubscriptions();
            RegisterPlatformDependencies();
            LoadApplication(new App());
            
            App.AuthenticationClient.PlatformParameters = new PlatformParameters(this);

            MakeStatusBarTranslucent(false);
            InitNFCService();
        }

        void InitNFCService()
        {
            StartService(new Intent(this, typeof(CardService)));
            DisableNFCService();
            MessagingCenter.Subscribe<string>(this, MessengerKeys.SendNFCToken, EnableNFCService);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(
              requestCode, resultCode, data);
        }

        void InitMessageCenterSubscriptions() => MessagingCenter.Instance.Subscribe<StatusBarHelper, bool>(this, StatusBarHelper.TranslucentStatusChangeMessage, OnTranslucentStatusRequest);

        void OnTranslucentStatusRequest(StatusBarHelper helper, bool makeTranslucent) => MakeStatusBarTranslucent(makeTranslucent);

        void MakeStatusBarTranslucent(bool makeTranslucent)
        {
            if (makeTranslucent)
            {
                SetStatusBarColor(Android.Graphics.Color.Transparent);

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.LayoutFullscreen | SystemUiFlags.LayoutStable);
                }
            }
            else
            {
                using (var value = new TypedValue())
                {
                    if (Theme.ResolveAttribute(Resource.Attribute.colorPrimaryDark, value, true))
                    {
                        var color = new Android.Graphics.Color(value.Data);
                        SetStatusBarColor(color);
                    }
                }

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Window.DecorView.SystemUiVisibility = StatusBarVisibility.Visible;
                }
            }
        }

        static void RegisterPlatformDependencies() => Locator.Instance.Register<IBrowserCookiesService, BrowserCookiesService>();

        void DisableNFCService()
        {
            var pm = PackageManager;
            pm.SetComponentEnabledSetting(
                new ComponentName(this, CardService.ServiceName),
                ComponentEnabledState.Disabled,
                ComponentEnableOption.DontKillApp);
        }

        void EnableNFCService(string message = "")
        {
            var pm = PackageManager;
            pm.SetComponentEnabledSetting(
                new ComponentName(this, CardService.ServiceName),
                ComponentEnabledState.Enabled,
                ComponentEnableOption.DontKillApp);
        }

        public override void OnBackPressed()
        {
            if (Popup.SendBackPressed(base.OnBackPressed) && 
                PopupNavigation.Instance.PopupStack.Any())
            {
                PopupNavigation.Instance.PopAllAsync(true);
            }
        }
    }
}