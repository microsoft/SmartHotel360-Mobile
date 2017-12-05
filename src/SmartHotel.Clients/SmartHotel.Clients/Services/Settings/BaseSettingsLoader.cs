using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Settings
{
    public interface IBaseSettingsLoader<TRemoteSettingsModel>
        where TRemoteSettingsModel : class
    {
        Task<TRemoteSettingsModel> LoadRemoteSettingsAsync(string fileUrl);
    }

    public abstract class BaseSettingsLoader<TRemoteSettingsModel> : IBaseSettingsLoader<TRemoteSettingsModel>
        where TRemoteSettingsModel : class
    {
        private readonly JsonSerializerSettings _serializerSettings;

        protected BaseSettingsLoader()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
        }

        public async Task<TRemoteSettingsModel> LoadRemoteSettingsAsync(string fileUrl)
        {
            System.Diagnostics.Debug.WriteLine($"Downloading remote settings from {fileUrl}");

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, fileUrl))
            using (var response = await client.SendAsync(request))
            {
                System.Diagnostics.Debug.WriteLine($"Received remote settings response. Succeeded? {response.IsSuccessStatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Remote settings download failed. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                }

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                {
                    var data = await reader.ReadToEndAsync();
                    return JsonConvert.DeserializeObject<TRemoteSettingsModel>(data);
                }
            }
        }
    }
}
