using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Services.Location;
using SmartHotel.Clients.Core.Services.Suggestion;
using SmartHotel.Clients.Core.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class SuggestionsViewModel : ViewModelBase
    {
        private ObservableCollection<CustomPin> _customPins;
        private ObservableCollection<Suggestion> _suggestions;

        private readonly ISuggestionService _suggestionService;
        private readonly ILocationService _locationService;

        public SuggestionsViewModel(
            ISuggestionService suggestionService, 
            ILocationService locationService)
        {
            _suggestionService = suggestionService;
            _locationService = locationService;
        }

        public ObservableCollection<CustomPin> CustomPins
        {
            get { return _customPins; }
            set
            {
                _customPins = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Suggestion> Suggestions
        {
            get { return _suggestions; }
            set
            {
                _suggestions = value;
                OnPropertyChanged();
            }
        }

        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                IsBusy = true;

                var location = await _locationService.GetPositionAsync();
                Suggestions = await _suggestionService.GetSuggestionsAsync(location.Latitude, location.Longitude);
                CustomPins = new ObservableCollection<CustomPin>();

                foreach (var suggestion in Suggestions)
                {
                    CustomPins.Add(new CustomPin
                    {
                        Label = suggestion.Name,
                        Position = new Xamarin.Forms.Maps.Position(suggestion.Latitude, suggestion.Longitude),
                        Type = suggestion.SuggestionType
                    });
                }
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"[Suggestions] Error retrieving data: {httpEx}");

                if (!string.IsNullOrEmpty(httpEx.Message))
                {
                    await DialogService.ShowAlertAsync( 
                        string.Format(Resources.HttpRequestExceptionMessage, httpEx.Message),
                        Resources.HttpRequestExceptionTitle,
                        Resources.DialogOk);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Suggestions] Error: {ex}");

                await DialogService.ShowAlertAsync(
                    Resources.ExceptionMessage,
                    Resources.ExceptionTitle,
                    Resources.DialogOk);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}