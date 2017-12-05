using System;
using Xamarin.UITest;
using Xamarin.UITest.Utils;

namespace SmartHotel.Clients.UITests
{
    static class AppManager
    {
        static IApp app;

        public static IApp App
        {
            get
            {
                if (app == null)
                    throw new NullReferenceException("'AppManager.App' not set. Call 'AppManager.StartApp()' before trying to access it.");
                return app;
            }
        }

        static Platform? platform;

        public static Platform Platform
        {
            get
            {
                if (platform == null)
                    throw new NullReferenceException("'AppManager.Platform' not set.");
                return platform.Value;
            }

            set
            {
                platform = value;
            }
        }

        public static void StartApp()
        {
            if (Platform == Platform.Android)
            {
                app = ConfigureApp
                    .Android
                    .WaitTimes(new WaitTimes())
                    // Used to run a .apk file:
                    .ApkFile("../../../SmartHotel.Clients/SmartHotel.Clients.Android/bin/Release/com.microsoft.smarthotel.apk")
                    .StartApp();
            }

            if (Platform == Platform.iOS)
            {
                app = ConfigureApp
                    .iOS
                    .WaitTimes(new WaitTimes())
                    // Used to run a .app file on an ios simulator:
                    //.AppBundle("path/to/file.app")
                    // Used to run a .ipa file on a physical ios device:
                    .InstalledApp("com.microsoft.SmartHotel360-df")
                    .StartApp();
            }
        }
    }

    public class WaitTimes : IWaitTimes
    {
        public TimeSpan GestureCompletionTimeout
        {
            get { return TimeSpan.FromMinutes(1); }
        }

        public TimeSpan GestureWaitTimeout
        {
            get { return TimeSpan.FromMinutes(1); }
        }

        public TimeSpan WaitForTimeout
        {
            get { return TimeSpan.FromMinutes(1); }
        }
    }
}