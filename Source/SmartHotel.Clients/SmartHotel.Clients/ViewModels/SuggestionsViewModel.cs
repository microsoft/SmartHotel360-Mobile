using MvvmHelpers;
using SmartHotel.Clients.Core.Exceptions;
using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Services.Geolocator;
using SmartHotel.Clients.Core.Services.Suggestion;
using SmartHotel.Clients.Core.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class SuggestionsViewModel : ViewModelBase
    {
        ObservableRangeCollection<CustomPin> customPins;
        ObservableRangeCollection<Suggestion> suggestions;

        readonly ISuggestionService suggestionService;
        readonly ILocationService locationService;

        public SuggestionsViewModel(
            ISuggestionService suggestionService,
            ILocationService locationService)
        {
            this.suggestionService = suggestionService;
            this.locationService = locationService;
        }

        public ObservableRangeCollection<CustomPin> CustomPins
        {
            get => customPins;
            set => SetProperty(ref customPins, value);
        }

        public ObservableRangeCollection<Suggestion> Suggestions
        {
            get => suggestions;
            set => SetProperty(ref suggestions, value);
        }

        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                IsBusy = true;

                var location = await locationService.GetPositionAsync();
                Suggestions = await suggestionService.GetSuggestionsAsync(location.Latitude, location.Longitude);
                CustomPins = new ObservableRangeCollection<CustomPin>();

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
            catch (ConnectivityException cex)
            {
                Debug.WriteLine($"[Suggestions] Connectivity Error: {cex}");
                await DialogService.ShowAlertAsync("There is no Internet conection, try again later.", "Error", "Ok");
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