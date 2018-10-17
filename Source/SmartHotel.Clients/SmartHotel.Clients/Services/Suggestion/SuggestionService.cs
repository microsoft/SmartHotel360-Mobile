using MvvmHelpers;
using SmartHotel.Clients.Core.Extensions;
using SmartHotel.Clients.Core.Services.Request;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Suggestion
{
    public class SuggestionService : ISuggestionService
    {
        readonly IRequestService requestService;

        public SuggestionService(IRequestService requestService)
        {
            this.requestService = requestService;
        }

        public async Task<ObservableRangeCollection<Models.Suggestion>> GetSuggestionsAsync(double latitude, double longitude)
        {
            var builder = new UriBuilder(AppSettings.SuggestionsEndpoint);
            builder.AppendToPath("suggestions");
            builder.Query = $"latitude={latitude.ToString(CultureInfo.InvariantCulture)}&longitude={longitude.ToString(CultureInfo.InvariantCulture)}";

            var uri = builder.ToString();
            var suggestions = await requestService.GetAsync<IEnumerable<Models.Suggestion>>(uri);

            return suggestions.ToObservableRangeCollection();
        }
    }
}