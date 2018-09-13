using Xamarin.Essentials;
using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Utils;

namespace SmartHotel.Clients.Core
{
    public static class AppSettings
    {
        //IF YOU DEPLOY YOUR OWN ENDPOINT REPLACE THE VALUEW BELOW
        //App Center
        const string defaultAppCenterAndroid = "b3b1403c-3f9d-4c77-805e-9c002de6ddf7";
        const string defaultAppCenteriOS = "7a2a290b-07b0-47dc-9dcd-15461e894e6d";
        const string defaultAppCenterUWP = "140a8550-c309-4bc1-a05d-e5a0f7e4df1d";

        // Endpoints
        const string defaultBookingEndpoint = "http://sh360services-public.eastus2.cloudapp.azure.com/bookings-api";
        const string defaultHotelsEndpoint = "http://sh360services-public.eastus2.cloudapp.azure.com/hotels-api";
        const string defaultSuggestionsEndpoint = "http://sh360services-public.eastus2.cloudapp.azure.com/suggestions-api";
        const string defaultNotificationsEndpoint = "http://sh360services-public.eastus2.cloudapp.azure.com/notifications-api";
        const string defaultSettingsFileUrl = "http://sh360services-public.eastus2.cloudapp.azure.com/configuration-api/cfg/public-http";
        const string defaultImagesBaseUri = "http://sh360imgpublic.blob.core.windows.net";

        // Maps
        const string defaultBingMapsApiKey = "9D6ZuqeGpcfZ9PVYR1BQ~ofsY_N_KDywcNM-Y0Io5aA~AvqaBtSnHxFfX7flAqux2Q6eYSIreLwDxnswabgPlEOXmoEXXt6u1O6In0hqICy8";
        public const string DefaultFallbackMapsLocation = "40.762246,-73.986943";

        // Bots
        const string defaultSkypeBotId = "897f3818-8da3-4d23-a613-9a0f9555f2ea";
        const string defaultFacebookBotId = "120799875283148";

        // B2c
        public const string B2cAuthority = "https://login.microsoftonline.com/";
        public const string DefaultB2cPolicy = "B2C_1_SignUpInPolicy";
        public const string DefaultB2cClientId = "b3cfbe11-ac36-4dcb-af16-8656ee286dcc";
        public const string DefaultB2cTenant = "smarthotel360.onmicrosoft.com";

        // Booking 
        const bool defaultHasBooking = false;

        // Fakes
        const bool defaultUseFakes = true;

        // Azure B2C settings
        public static string B2cClientId
        {
            get => Preferences.Get(nameof(B2cClientId), DefaultB2cClientId);
            set => Preferences.Set(nameof(B2cClientId), value);
        }

        public static string B2cTenant
        {
            get => Preferences.Get(nameof(B2cTenant), DefaultB2cTenant);
            set => Preferences.Set(nameof(B2cTenant), value);
        }

        public static string B2cPolicy
        {
            get => Preferences.Get(nameof(B2cPolicy), DefaultB2cPolicy);
            set => Preferences.Set(nameof(B2cPolicy), value);
        }
        

        // API Endpoints
        public static string BookingEndpoint
        {
            get => Preferences.Get(nameof(BookingEndpoint), defaultBookingEndpoint);
            set => Preferences.Set(nameof(BookingEndpoint), value);
        }

        public static string HotelsEndpoint
        {
            get => Preferences.Get(nameof(HotelsEndpoint), defaultHotelsEndpoint);
            set => Preferences.Set(nameof(HotelsEndpoint), value);
        }

        public static string SuggestionsEndpoint
        {
            get => Preferences.Get(nameof(SuggestionsEndpoint), defaultSuggestionsEndpoint);
            set => Preferences.Set(nameof(SuggestionsEndpoint), value);
        }

        public static string NotificationsEndpoint
        {
            get => Preferences.Get(nameof(NotificationsEndpoint), defaultNotificationsEndpoint);
            set => Preferences.Set(nameof(NotificationsEndpoint), value);
        }

        public static string ImagesBaseUri
        {
            get => Preferences.Get(nameof(ImagesBaseUri), defaultImagesBaseUri);
            set => Preferences.Set(nameof(ImagesBaseUri), value);
        }

        public static string SkypeBotId
        {
            get => Preferences.Get(nameof(SkypeBotId), defaultSkypeBotId);
            set => Preferences.Set(nameof(SkypeBotId), value);
        }

        public static string FacebookBotId
        {
            get => Preferences.Get(nameof(FacebookBotId), defaultFacebookBotId);
            set => Preferences.Set(nameof(FacebookBotId), value);
        }

        // Other settings

        public static string BingMapsApiKey
        {
            get => Preferences.Get(nameof(BingMapsApiKey), defaultBingMapsApiKey);
            set => Preferences.Set(nameof(BingMapsApiKey), value);
        }

        public static string SettingsFileUrl
        {
            get => Preferences.Get(nameof(SettingsFileUrl), defaultSettingsFileUrl);
            set => Preferences.Set(nameof(SettingsFileUrl), value);
        }

        public static string FallbackMapsLocation
        {
            get => Preferences.Get(nameof(FallbackMapsLocation), DefaultFallbackMapsLocation);
            set => Preferences.Set(nameof(FallbackMapsLocation), value);
        }

        public static User User
        {
            get => PreferencesHelpers.Get(nameof(User), default(User));
            set => PreferencesHelpers.Set(nameof(User), value);
        }

        public static string AppCenterAnalyticsAndroid
        {
            get => Preferences.Get(nameof(AppCenterAnalyticsAndroid), defaultAppCenterAndroid);
            set => Preferences.Set(nameof(AppCenterAnalyticsAndroid), value);
        }

        public static string AppCenterAnalyticsIos
        {
            get => Preferences.Get(nameof(AppCenterAnalyticsIos), defaultAppCenteriOS);
            set => Preferences.Set(nameof(AppCenterAnalyticsIos), value);
        }

        public static string AppCenterAnalyticsWindows
        {
            get => Preferences.Get(nameof(AppCenterAnalyticsWindows), defaultAppCenterUWP);
            set => Preferences.Set(nameof(AppCenterAnalyticsWindows), value);
        }

        public static bool UseFakes
        {
            get => Preferences.Get(nameof(UseFakes), defaultUseFakes);
            set => Preferences.Set(nameof(UseFakes), value);
        }

        public static bool HasBooking
        {
            get => Preferences.Get(nameof(HasBooking), defaultHasBooking);
            set => Preferences.Set(nameof(HasBooking), value);
        }

        public static void RemoveUserData() => Preferences.Remove(nameof(User));
    }
}