using Plugin.Settings;
using Plugin.Settings.Abstractions;
using SmartHotel.Clients.Core.Extensions;
using SmartHotel.Clients.Core.Models;

namespace SmartHotel.Clients.Core
{
    public static class AppSettings
    {
        //IF YOU DEPLOY YOUR OWN ENDPOINT REPLACE THE VALUEW BELOW
        //App Center
        private const string DefaultAppCenterAndroid = "b3b1403c-3f9d-4c77-805e-9c002de6ddf7";
        private const string DefaultAppCenteriOS = "7a2a290b-07b0-47dc-9dcd-15461e894e6d";
        private const string DefaultAppCenterUWP = "140a8550-c309-4bc1-a05d-e5a0f7e4df1d";

        // Endpoints
        private const string DefaultBookingEndpoint = "http://sh360services-public.eastus2.cloudapp.azure.com/bookings-api";
        private const string DefaultHotelsEndpoint = "http://sh360services-public.eastus2.cloudapp.azure.com/hotels-api";
        private const string DefaultSuggestionsEndpoint = "http://sh360services-public.eastus2.cloudapp.azure.com/suggestions-api";
        private const string DefaultNotificationsEndpoint = "http://sh360services-public.eastus2.cloudapp.azure.com/notifications-api";
        private const string DefaultSettingsFileUrl = "http://sh360services-public.eastus2.cloudapp.azure.com/configuration-api/cfg/public-http";
        private const string DefaultImagesBaseUri = "http://sh360imgpublic.blob.core.windows.net";

        // Maps
        private const string DefaultBingMapsApiKey = "9D6ZuqeGpcfZ9PVYR1BQ~ofsY_N_KDywcNM-Y0Io5aA~AvqaBtSnHxFfX7flAqux2Q6eYSIreLwDxnswabgPlEOXmoEXXt6u1O6In0hqICy8";
        public const string DefaultFallbackMapsLocation = "40.762246,-73.986943";

        // Bots
        private const string DefaultSkypeBotId = "897f3818-8da3-4d23-a613-9a0f9555f2ea";
        private const string DefaultFacebookBotId = "120799875283148";

        // B2c
        public const string B2cAuthority = "https://login.microsoftonline.com/";
        public const string DefaultB2cPolicy = "B2C_1_SignUpInPolicy";
        public const string DefaultB2cClientId = "b3cfbe11-ac36-4dcb-af16-8656ee286dcc";
        public const string DefaultB2cTenant = "smarthotel360.onmicrosoft.com";

        // Booking 
        private const bool DefaultHasBooking = false;

        // Fakes
        private const bool DefaultUseFakes = true;

        private static ISettings Settings => CrossSettings.Current;

        // Azure B2C settings
        public static string B2cClientId
        {
            get => Settings.GetValueOrDefault(nameof(B2cClientId), DefaultB2cClientId);

            set => Settings.AddOrUpdateValue(nameof(B2cClientId), value);
        }

        public static string B2cTenant
        {
            get => Settings.GetValueOrDefault(nameof(B2cTenant), DefaultB2cTenant);

            set => Settings.AddOrUpdateValue(nameof(B2cTenant), value);
        }

        public static string B2cPolicy
        {
            get => Settings.GetValueOrDefault(nameof(B2cPolicy), DefaultB2cPolicy);

            set => Settings.AddOrUpdateValue(nameof(B2cPolicy), value);
        }
        

        // API Endpoints
        public static string BookingEndpoint
        {
            get => Settings.GetValueOrDefault(nameof(BookingEndpoint), DefaultBookingEndpoint);

            set => Settings.AddOrUpdateValue(nameof(BookingEndpoint), value);
        }

        public static string HotelsEndpoint
        {
            get => Settings.GetValueOrDefault(nameof(HotelsEndpoint), DefaultHotelsEndpoint);

            set => Settings.AddOrUpdateValue(nameof(HotelsEndpoint), value);
        }

        public static string SuggestionsEndpoint
        {
            get => Settings.GetValueOrDefault(nameof(SuggestionsEndpoint), DefaultSuggestionsEndpoint);

            set => Settings.AddOrUpdateValue(nameof(SuggestionsEndpoint), value);
        }

        public static string NotificationsEndpoint
        {
            get => Settings.GetValueOrDefault(nameof(NotificationsEndpoint), DefaultNotificationsEndpoint);

            set => Settings.AddOrUpdateValue(nameof(NotificationsEndpoint), value);
        }

        public static string ImagesBaseUri
        {
            get => Settings.GetValueOrDefault(nameof(ImagesBaseUri), DefaultImagesBaseUri);

            set => Settings.AddOrUpdateValue(nameof(ImagesBaseUri), value);
        }

        public static string SkypeBotId
        {
            get => Settings.GetValueOrDefault(nameof(SkypeBotId), DefaultSkypeBotId);

            set => Settings.AddOrUpdateValue(nameof(SkypeBotId), value);
        }

        public static string FacebookBotId
        {
            get => Settings.GetValueOrDefault(nameof(FacebookBotId), DefaultFacebookBotId);

            set => Settings.AddOrUpdateValue(nameof(FacebookBotId), value);
        }

        // Other settings

        public static string BingMapsApiKey
        {
            get => Settings.GetValueOrDefault(nameof(BingMapsApiKey), DefaultBingMapsApiKey);

            set => Settings.AddOrUpdateValue(nameof(BingMapsApiKey), value);
        }

        public static string SettingsFileUrl
        {
            get => Settings.GetValueOrDefault(nameof(SettingsFileUrl), DefaultSettingsFileUrl);

            set => Settings.AddOrUpdateValue(nameof(SettingsFileUrl), value);
        }

        public static string FallbackMapsLocation
        {
            get => Settings.GetValueOrDefault(nameof(FallbackMapsLocation), DefaultFallbackMapsLocation);

            set => Settings.AddOrUpdateValue(nameof(FallbackMapsLocation), value);
        }

        public static User User
        {
            get => Settings.GetValueOrDefault(nameof(User), default(User));

            set => Settings.AddOrUpdateValue(nameof(User), value);
        }

        public static string AppCenterAnalyticsAndroid
        {
            get => Settings.GetValueOrDefault(nameof(AppCenterAnalyticsAndroid), DefaultAppCenterAndroid);

            set => Settings.AddOrUpdateValue(nameof(AppCenterAnalyticsAndroid), value);
        }

        public static string AppCenterAnalyticsIos
        {
            get => Settings.GetValueOrDefault(nameof(AppCenterAnalyticsIos), DefaultAppCenteriOS);

            set => Settings.AddOrUpdateValue(nameof(AppCenterAnalyticsIos), value);
        }

        public static string AppCenterAnalyticsWindows
        {
            get => Settings.GetValueOrDefault(nameof(AppCenterAnalyticsWindows), DefaultAppCenterUWP);

            set => Settings.AddOrUpdateValue(nameof(AppCenterAnalyticsWindows), value);
        }

        public static bool UseFakes
        {
            get => Settings.GetValueOrDefault(nameof(UseFakes), DefaultUseFakes);

            set => Settings.AddOrUpdateValue(nameof(UseFakes), value);
        }

        public static bool HasBooking
        {
            get => Settings.GetValueOrDefault(nameof(HasBooking), DefaultHasBooking);

            set => Settings.AddOrUpdateValue(nameof(HasBooking), value);
        }

        public static void RemoveUserData()
        {
            Settings.Remove(nameof(User));
        }
    }
}