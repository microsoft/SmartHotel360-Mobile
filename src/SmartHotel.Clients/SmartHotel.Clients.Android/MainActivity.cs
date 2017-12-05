using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using CarouselView.FormsPlugin.Android;
using Microsoft.Identity.Client;
using Plugin.Permissions;
using SmartHotel.Clients.Core;
using SmartHotel.Clients.Core.Helpers;
using SmartHotel.Clients.Core.Services.Authentication;
using SmartHotel.Clients.Core.ViewModels.Base;
using SmartHotel.Clients.Droid.Services.Authentication;
using SmartHotel.Clients.Droid.Services.CardEmulation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

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
            UserDialogs.Init(this);
            Renderers.Calendar.Init();
            Xamarin.FormsMaps.Init(this, bundle);

            InitMessageCenterSubscriptions();
            RegisterPlatformDependencies();
            LoadApplication(new App());

            App.AuthenticationClient.PlatformParameters =
                  new PlatformParameters(Forms.Context as Activity);

            MakeStatusBarTranslucent(false);
            InitNFCService();
        }

        private void InitNFCService()
        {
            StartService(new Intent(this, typeof(CardService)));
            DisableNFCService();
            MessagingCenter.Subscribe<string>(this, MessengerKeys.SendNFCToken, EnableNFCService);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(
              requestCode, resultCode, data);
        }

        private void InitMessageCenterSubscriptions()
        {
            MessagingCenter.Instance.Subscribe<StatusBarHelper, bool>(this, StatusBarHelper.TranslucentStatusChangeMessage, OnTranslucentStatusRequest);
        }

        private void OnTranslucentStatusRequest(StatusBarHelper helper, bool makeTranslucent)
        {
            MakeStatusBarTranslucent(makeTranslucent);
        }

        private void MakeStatusBarTranslucent(bool makeTranslucent)
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

        private static void RegisterPlatformDependencies()
        {
            Locator.Instance.Register<IBrowserCookiesService, BrowserCookiesService>();
        }

        private void DisableNFCService()
        {
            PackageManager pm = this.PackageManager;
            pm.SetComponentEnabledSetting(
                new ComponentName(this, CardService.ServiceName),
                ComponentEnabledState.Disabled,
                ComponentEnableOption.DontKillApp);
        }

        private void EnableNFCService(string message = "")
        {
            PackageManager pm = this.PackageManager;
            pm.SetComponentEnabledSetting(
                new ComponentName(this, CardService.ServiceName),
                ComponentEnabledState.Enabled,
                ComponentEnableOption.DontKillApp);
        }
    }
}