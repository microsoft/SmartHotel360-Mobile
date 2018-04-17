using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Nfc;
using Android.OS;
using Android.Views;
using Android.Runtime;
using Android.Util;
using FFImageLoading;
using FFImageLoading.Forms.Droid;
using System;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace SmartHotel.Clients.NFC.Droid
{
    [Activity(
        Label = "SmartHotel NFC",
        Icon = "@drawable/icon",
        Theme = "@style/MainTheme",
        MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, SmartHotelCardReader.MessageCallback
    {
        // Recommend NfcAdapter flags for reading from other Android devices. Indicates that this
        // activity is interested in NFC-A devices (including other Android devices), and that the
        // system should not check for the presence of NDEF-formatted data (e.g. Android Beam).
        public NfcReaderFlags READER_FLAGS = NfcReaderFlags.NfcA | NfcReaderFlags.SkipNdefCheck;
        public SmartHotelCardReader _smartHotelCardReader;

        public string TAG
        {
            get { return "CardReader"; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            //APP CENTER KEY
            AppCenter.Start("88002d1c-ad2f-46e4-8233-79561b596601",
                   typeof(Analytics), typeof(Crashes));

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            CachedImageRenderer.Init();
            LoadApplication(new App());

            _smartHotelCardReader = new SmartHotelCardReader(new WeakReference<SmartHotelCardReader.MessageCallback>(this));

            // Disable Android Beam and register our card reader callback
            EnableReaderMode();

            this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
        }

        protected override void OnPause()
        {
            base.OnPause();

            ImageService.Instance.SetExitTasksEarly(true);

            DisableReaderMode();
        }

        protected override void OnResume()
        {
            base.OnResume();

            ImageService.Instance.SetExitTasksEarly(false);

            EnableReaderMode();
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            ImageService.Instance.InvalidateMemoryCache();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            base.OnTrimMemory(level);
        }

        private void EnableReaderMode()
        {
            Log.Info(TAG, "Enabling reader mode");

            Activity activity = this;
            NfcAdapter nfc = NfcAdapter.GetDefaultAdapter(activity);
            if (nfc != null)
            {
                nfc.EnableReaderMode(activity, _smartHotelCardReader, READER_FLAGS, null);
            }
        }

        private void DisableReaderMode()
        {
            Log.Info(TAG, "Enabling reader mode");

            Activity activity = this;
            NfcAdapter nfc = NfcAdapter.GetDefaultAdapter(activity);
            if (nfc != null)
            {
                nfc.DisableReaderMode(activity);
            }
        }


        public void OnMessageRecieved(string message)
        {
            RunOnUiThread(() =>
            {
                Log.Info(TAG, message);
                MessagingCenter.Send(message, MessengerKeys.SendNFCToken);
            });
        }
    }
}