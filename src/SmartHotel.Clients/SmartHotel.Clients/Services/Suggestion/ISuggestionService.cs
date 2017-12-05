using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Suggestion
{
    public interface ISuggestionService
    {
        Task<ObservableCollection<Models.Suggestion>> GetSuggestionsAsync(double latitude, double longitude);
    }
}