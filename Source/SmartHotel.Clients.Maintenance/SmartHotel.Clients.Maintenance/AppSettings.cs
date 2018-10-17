using Xamarin.Essentials;

namespace SmartHotel.Clients.Maintenance
{
    public static class AppSettings
    {
        const string DefaultSettingsFileUrl = "http://sh360services-public.eastus2.cloudapp.azure.com/configuration-api/cfg/public-http";
        const string DefaulTasksEndpoint = "http://sh360services-public.eastus2.cloudapp.azure.com/tasks-api";
        const string DefaultAppCenteriOS = "857463fc-96d4-49c4-b5b8-21ef22685b9f";

        public static string Animation = "PopupAnimation";

        // API Endpoints
        public static string TasksEndpoint
        {
            get => Preferences.Get(nameof(TasksEndpoint), DefaulTasksEndpoint);

            set => Preferences.Set(nameof(TasksEndpoint), value);
        }

        public static string SettingsFileUrl
        {
            get => Preferences.Get(nameof(SettingsFileUrl), DefaultSettingsFileUrl);

            set => Preferences.Set(nameof(SettingsFileUrl), value);
        }

        public static string AppCenterAnalyticsIos
        {
            get => Preferences.Get(nameof(AppCenterAnalyticsIos), DefaultAppCenteriOS);

            set => Preferences.Set(nameof(AppCenterAnalyticsIos), value);
        }
    }
}