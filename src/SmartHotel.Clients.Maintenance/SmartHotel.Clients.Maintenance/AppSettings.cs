using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace SmartHotel.Clients.Maintenance
{
    public static class AppSettings
    {
        private const string DefaultSettingsFileUrl = "http://sh360services-public.eastus2.cloudapp.azure.com/configuration-api/cfg/public-http";
        private const string DefaulTasksEndpoint = "YOUR_TASKS_ENDPOINT";

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
    }
}