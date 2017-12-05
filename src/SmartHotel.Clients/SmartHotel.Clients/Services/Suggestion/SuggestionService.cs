using SmartHotel.Clients.Core.Extensions;
using SmartHotel.Clients.Core.Services.Request;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Suggestion
{
    public class SuggestionService : ISuggestionService
    {
        private readonly IRequestService _requestService;

        public SuggestionService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<ObservableCollection<Models.Suggestion>> GetSuggestionsAsync(double latitude, double longitude)
        {
            UriBuilder builder = new UriBuilder(AppSettings.SuggestionsEndpoint);
            builder.AppendToPath("suggestions");
            builder.Query = $"latitude={latitude.ToString(CultureInfo.InvariantCulture)}&longitude={longitude.ToString(CultureInfo.InvariantCulture)}";

            string uri = builder.ToString();
            IEnumerable<Models.Suggestion> suggestions = await _requestService.GetAsync<IEnumerable<Models.Suggestion>>(uri);

            return suggestions.ToObservableCollection();
        }
    }
}