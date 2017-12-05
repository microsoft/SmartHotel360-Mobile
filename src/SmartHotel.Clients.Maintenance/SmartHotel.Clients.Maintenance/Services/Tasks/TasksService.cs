using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SmartHotel.Clients.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace SmartHotel.Clients.Maintenance.Services.Tasks
{
    public class TasksService : ITasksService
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public TasksService()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };

            _serializerSettings.Converters.Add(new StringEnumConverter());
        }
        public async Task<IEnumerable<Models.Task>> GetTasksAsync()
        {
            UriBuilder builder = new UriBuilder(AppSettings.TasksEndpoint);
            builder.AppendToPath("tasks");

            string uri = builder.ToString();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            var serialized = await response.Content.ReadAsStringAsync();

            var result = await Task.Run(() => 
            JsonConvert.DeserializeObject<IEnumerable<Models.Task>>(serialized, _serializerSettings));

            return result;
        }

        public async Task MarkTaskAsResolvedAsync(int taskId)
        {
            UriBuilder builder = new UriBuilder(AppSettings.TasksEndpoint);
            builder.AppendToPath($"tasks/resolved/{taskId}");

            string uri = builder.ToString();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await httpClient.PutAsync(uri, null);
        }
    }
}