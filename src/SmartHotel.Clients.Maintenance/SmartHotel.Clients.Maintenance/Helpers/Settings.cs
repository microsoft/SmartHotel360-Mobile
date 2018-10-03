// Helpers/Settings.cs

using Xamarin.Essentials;

namespace SmartHotel.Clients.Maintenance.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
	{
		#region Setting Constants

		const string SettingsKey = "settings_key";
		static readonly string SettingsDefault = string.Empty;

		#endregion


		public static string GeneralSettings
        {
            get => Preferences.Get(SettingsKey, SettingsDefault);
            set => Preferences.Set(SettingsKey, value);
        }

    }
}