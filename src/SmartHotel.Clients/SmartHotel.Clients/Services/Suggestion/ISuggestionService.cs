using System.Threading.Tasks;
using MvvmHelpers;

namespace SmartHotel.Clients.Core.Services.Suggestion
{
    public interface ISuggestionService
    {
        Task<ObservableRangeCollection<Models.Suggestion>> GetSuggestionsAsync(double latitude, double longitude);
    }
}