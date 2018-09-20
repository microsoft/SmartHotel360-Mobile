using System.Threading.Tasks;
using SmartHotel.Clients.Core.Services.Hotel;
using SmartHotel.Clients.Core.ViewModels.Base;
using System.Collections.Generic;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Net.Http;
using System.Diagnostics;
using MvvmHelpers;
using SmartHotel.Clients.Core.Extensions;
using SmartHotel.Clients.Core.Exceptions;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class BookingHotelsViewModel : ViewModelBase
    {
        ObservableRangeCollection<Models.Hotel> hotels;
        Models.City city;
        DateTime from;
        DateTime until;

        readonly IHotelService hotelService;

        public BookingHotelsViewModel(IHotelService hotelService) => this.hotelService = hotelService;

        public Models.City City
        {
            get => city;
            set => SetProperty(ref city, value);
        }

        public DateTime From
        {
            get => from;
            set => SetProperty(ref from, value);
        }

        public DateTime Until
        {
            get => until;
            set => SetProperty(ref until, value);
        }

        public ObservableRangeCollection<Models.Hotel> Hotels
        {
            get => hotels;
            set => SetProperty(ref hotels, value);
        }

        public ICommand HotelSelectedCommand => new Command<Models.Hotel>(OnSelectHotelAsync);

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData != null)
            {
                var navigationParameter = navigationData as Dictionary<string, object>;
                City = navigationParameter["city"] as Models.City;
                From = (DateTime)navigationParameter["from"];
                Until = (DateTime)navigationParameter["until"];
            }

            try
            {
                IsBusy = true;

                var hotels = await hotelService.SearchAsync(City.Id);
                Hotels = hotels.ToObservableRangeCollection();
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"[Booking Hotels Step] Error retrieving data: {httpEx}");

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
                Debug.WriteLine($"[Booking Hotels Step] Connectivity Error: {cex}");
                await DialogService.ShowAlertAsync("There is no Internet conection, try again later.", "Error", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Booking Hotels Step] Error: {ex}");

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

        async void OnSelectHotelAsync(Models.Hotel item)
        {
            if (item != null)
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { "hotel", item },
                    { "from", From },
                    { "until", Until },
                };

                await NavigationService.NavigateToAsync<BookingHotelViewModel>(navigationParameter);
            }
        }
    }
}