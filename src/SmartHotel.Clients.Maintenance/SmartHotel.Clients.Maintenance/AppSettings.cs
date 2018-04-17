using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace SmartHotel.Clients.Maintenance
{
    public static class AppSettings
    {
        private const string DefaultSettingsFileUrl = "http://sh360services-public.eastus2.cloudapp.azure.com/configuration-api/cfg/public-http";
        private const string DefaulTasksEndpoint = "http://sh360services-public.eastus2.cloudapp.azure.com/tasks-api";

        private const string DefaultAppCenteriOS = "857463fc-96d4-49c4-b5b8-21ef22685b9f";

        private static ISettings Settings => CrossSettings.Current;

        public static string Animation = "PopupAnimation";

        // API Endpoints
        public static string TasksEndpoint
        {
            get => Settings.GetValueOrDefault(nameof(TasksEndpoint), DefaulTasksEndpoint);

            set => Settings.AddOrUpdateValue(nameof(TasksEndpoint), value);
        }

        public static string SettingsFileUrl
        {
            get => Settings.GetValueOrDefault(nameof(SettingsFileUrl), DefaultSettingsFileUrl);

            set => Settings.AddOrUpdateValue(nameof(SettingsFileUrl), value);
        }

        public static string AppCenterAnalyticsIos
        {
            get => Settings.GetValueOrDefault(nameof(AppCenterAnalyticsIos), DefaultAppCenteriOS);

            set => Settings.AddOrUpdateValue(nameof(AppCenterAnalyticsIos), value);
        }
    }
}