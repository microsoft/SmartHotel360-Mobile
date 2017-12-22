using System.Threading.Tasks;
using SmartHotel.Clients.Core.Services.Hotel;
using SmartHotel.Clients.Core.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Net.Http;
using System.Diagnostics;
using SmartHotel.Clients.Core.Extensions;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class BookingHotelsViewModel : ViewModelBase
    {
        private ObservableCollection<Models.Hotel> _hotels;
        private Models.City _city;
        private DateTime _from;
        private DateTime _until;

        private readonly IHotelService _hotelService;

        public BookingHotelsViewModel(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public Models.City City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }

        public DateTime From
        {
            get { return _from; }
            set
            {
                _from = value;
                OnPropertyChanged();
            }
        }

        public DateTime Until
        {
            get { return _until; }
            set
            {
                _until = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Models.Hotel> Hotels
        {
            get { return _hotels; }
            set
            {
                _hotels = value;
                OnPropertyChanged();
            }
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

                var hotels = await _hotelService.SearchAsync(City.Id);
                Hotels = hotels.ToObservableCollection();
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

        private async void OnSelectHotelAsync(Models.Hotel item)
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