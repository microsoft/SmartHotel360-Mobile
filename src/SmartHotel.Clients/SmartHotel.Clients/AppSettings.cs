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

        static string defaultBookingEndpoint;
        static string defaultHotelsEndpoint;
        static string defaultSuggestionsEndpoint;
        static string defaultNotificationsEndpoint;
        static string defaultSettingsFileUrl;

        // Endpoints
        const string defaultImagesBaseUri = "https://sh360publicimg.blob.core.windows.net";
        const string defaultRoomDevicesEndpoint = "";

        // Maps
        const string defaultBingMapsApiKey = "9D6ZuqeGpcfZ9PVYR1BQ~ofsY_N_KDywcNM-Y0Io5aA~AvqaBtSnHxFfX7flAqux2Q6eYSIreLwDxnswabgPlEOXmoEXXt6u1O6In0hqICy8";
        public const string DefaultFallbackMapsLocation = "40.762246,-73.986943";

        // Bots
        const string defaultSkypeBotId = "87e0cdb5-8e79-4592-9dc8-11697ffe79cc";

        // B2c
        public const string B2cAuthority = "https://login.microsoftonline.com/";
        public const string DefaultB2cPolicy = "B2C_1_SignUpInPolicy";
        public const string DefaultB2cClientId = "b3cfbe11-ac36-4dcb-af16-8656ee286dcc";
        public const string DefaultB2cTenant = "smarthotel360.onmicrosoft.com";

        // Booking 
        const bool defaultHasBooking = false;

		// Room Devices
	    const string defaultRoomId = "";
        const string defaultThermostatDeviceId = "Room11Thermostat";
        const string defaultLightDeviceId = "Room11Light";


        // Fakes
        const bool defaultUseFakes = false;

        static AppSettings()
        {           
            defaultBookingEndpoint = "http://myapp.b967f3ac20524797b96a.eastus.aksapp.io/bookings";
            defaultHotelsEndpoint = "http://myapp.b967f3ac20524797b96a.eastus.aksapp.io/hotels-api";
            defaultSuggestionsEndpoint = "http://myapp.b967f3ac20524797b96a.eastus.aksapp.io/suggestions-api";
            defaultNotificationsEndpoint = "http://myapp.b967f3ac20524797b96a.eastus.aksapp.io/notifications-api";
            defaultSettingsFileUrl = "http://myapp.b967f3ac20524797b96a.eastus.aksapp.io/configuration-api/cfg/public";
		}

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

        public static string RoomDevicesEndpoint
        {
            get => Preferences.Get(nameof(RoomDevicesEndpoint), defaultRoomDevicesEndpoint);
            set => Preferences.Set(nameof(RoomDevicesEndpoint), value);
        }

        public static string SkypeBotId
        {
            get => Preferences.Get(nameof(SkypeBotId), defaultSkypeBotId);
            set => Preferences.Set(nameof(SkypeBotId), value);
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
		
		public static string RoomId
	    {
		    get => Preferences.Get(nameof(RoomId), defaultRoomId);
		    set => Preferences.Set(nameof(RoomId), value);
	    }

        public static string ThermostatDeviceId
        {
            get => Preferences.Get(nameof(ThermostatDeviceId), defaultThermostatDeviceId);
            set => Preferences.Set(nameof(ThermostatDeviceId), value);
        }

        public static string LightDeviceId
        {
            get => Preferences.Get(nameof(LightDeviceId), defaultLightDeviceId);
            set => Preferences.Set(nameof(LightDeviceId), value);
        }
    }
}