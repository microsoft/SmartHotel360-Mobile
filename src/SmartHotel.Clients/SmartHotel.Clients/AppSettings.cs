using Plugin.Settings;
using Plugin.Settings.Abstractions;
using SmartHotel.Clients.Core.Extensions;
using SmartHotel.Clients.Core.Models;

namespace SmartHotel.Clients.Core
{
    public static class AppSettings
    {
        // Endpoints
        private const string DefaultBookingEndpoint = "YOUR_BOOKING_ENDPOINT";
        private const string DefaultHotelsEndpoint = "YOUR_HOTELS_ENDPOINT";
        private const string DefaultSuggestionsEndpoint = "YOUR_SUGGESTIONS_ENDPOINT";
        private const string DefaultNotificationsEndpoint = "YOUR_NOTIFICATIONS_ENDPOINT";
        private const string DefaultSettingsFileUrl = "http://sh360services-public.eastus2.cloudapp.azure.com/configuration-api/cfg/public-http";
        private const string DefaultImagesBaseUri = "YOUR_IMAGE_BASE_URI";

        // Mobile Center
        private const string DefaultMobileCenterAnalyticsAndroid = "YOUR_APPCENTER_ANDROID_ANALYTICS_ID";
        private const string DefaultMobileCenterAnalyticsIos = "YOUR_APPCENTER_IOS_ANALYTICS_ID";
        private const string DefaultMobileCenterAnalyticsWindows = "YOUR_APPCENTER_WINDOWS_ANALYTICS_ID";

        // Maps
        private const string DefaultBingMapsApiKey = "YOUR_BINGMAPS_API_KEY";
        public const string DefaultFallbackMapsLocation = "40.762246 -73.986943";

        // Bots
        private const string DefaultSkypeBotId = "YOUR_SKYPE_BOT_ID";
        private const string DefaultFacebookBotId = "YOUR_FACEBOOK_BOT_ID";

        // B2c
        public const string B2cAuthority = "https://login.microsoftonline.com/";
        public const string DefaultB2cPolicy = "B2C_1_SignUpInPolicy";
        public const string DefaultB2cClientId = "YOUR_B2C_CLIENT_ID";
        public const string DefaultB2cTenant = "YOUR_B2C_TENANT";

        // Booking 
        private const bool DefaultHasBooking = false;

        // Fakes
        private const bool DefaultUseFakes = false;

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

        public static string MobileCenterAnalyticsAndroid
        {
            get => Settings.GetValueOrDefault(nameof(MobileCenterAnalyticsAndroid), DefaultMobileCenterAnalyticsAndroid);

            set => Settings.AddOrUpdateValue(nameof(MobileCenterAnalyticsAndroid), value);
        }

        public static string MobileCenterAnalyticsIos
        {
            get => Settings.GetValueOrDefault(nameof(MobileCenterAnalyticsIos), DefaultMobileCenterAnalyticsIos);

            set => Settings.AddOrUpdateValue(nameof(MobileCenterAnalyticsIos), value);
        }

        public static string MobileCenterAnalyticsWindows
        {
            get => Settings.GetValueOrDefault(nameof(MobileCenterAnalyticsWindows), DefaultMobileCenterAnalyticsWindows);

            set => Settings.AddOrUpdateValue(nameof(MobileCenterAnalyticsWindows), value);
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