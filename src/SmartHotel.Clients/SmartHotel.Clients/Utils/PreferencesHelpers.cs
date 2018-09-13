using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using Xamarin.Essentials;

namespace SmartHotel.Clients.Core.Utils
{
    public static class PreferencesHelpers
    {
        public static T Get<T>(string key, T @default) where T : class
        {
            var serialized = Preferences.Get(key, string.Empty);
            var result = @default;

            try
            {
                var serializeSettings = GetSerializerSettings();
                result = JsonConvert.DeserializeObject<T>(serialized);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deserializing settings value: {ex}");
            }

            return result;
        }


        public static void Set<T>(string key, T obj) where T : class
        {
            try
            {
                var serializeSettings = GetSerializerSettings();
                var serialized = JsonConvert.SerializeObject(obj, serializeSettings);

                Preferences.Set(key, serialized);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error serializing settings value: {ex}");
            }
        }

        static JsonSerializerSettings GetSerializerSettings() => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}
