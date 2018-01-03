using System.Threading.Tasks;
using SmartHotel.Clients.Core.ViewModels.Base;
using System.Windows.Input;
using Xamarin.Forms;
using SmartHotel.Clients.Core.Services.Hotel;
using System.Collections.ObjectModel;
using System;
using System.Diagnostics;
using SmartHotel.Clients.Core.Extensions;
using System.Net.Http;
using SmartHotel.Clients.Core.Services.Booking;
using SmartHotel.Clients.Core.Services.Authentication;
using System.Collections.Generic;
using System.Linq;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class BookingHotelViewModel : ViewModelBase
    {
        private bool _myHotel;
        private bool _rooms;
        private bool _reviews;
        private Models.Hotel _hotel;
        private DateTime _from;
        private DateTime _until;
        private ObservableCollection<Models.Service> _hotelServices;
        private ObservableCollection<Models.Service> _roomServices;
        private ObservableCollection<Models.Review> _hotelReviews;

        private readonly IHotelService _hotelService;
        private readonly IBookingService _bookingService;
        private readonly IAuthenticationService _authenticationService;

        public BookingHotelViewModel(
            IHotelService hotelService,
            IBookingService bookingService,
            IAuthenticationService authenticationService)
        {
            _hotelService = hotelService;
            _bookingService = bookingService;
            _authenticationService = authenticationService;

            SetMyHotel();
        }

        public Models.Hotel Hotel
        {
            get { return _hotel; }
            set
            {
                _hotel = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Models.Service> HotelServices
        {
            get { return _hotelServices; }
            set
            {
                _hotelServices = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Models.Service> RoomServices
        {
            get { return _roomServices; }
            set
            {
                _roomServices = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Models.Review> HotelReviews
        {
            get { return _hotelReviews; }
            set
            {
                _hotelReviews = value;
                OnPropertyChanged();
            }
        }

        public bool MyHotel
        {
            get { return _myHotel; }
            set
            {
                _myHotel = value;
                OnPropertyChanged();
            }
        }

        public bool Rooms
        {
            get { return _rooms; }
            set
            {
                _rooms = value;
                OnPropertyChanged();
            }
        }

        public bool Reviews
        {
            get { return _reviews; }
            set
            {
                _reviews = value;
                OnPropertyChanged();
            }
        }

        public ICommand MyHotelCommand => new Command(SetMyHotel);

        public ICommand RoomsCommand => new Command(SetRooms);

        public ICommand ReviewsCommand => new Command(SetReviews);

        public ICommand BookingCommand => new AsyncCommand(BookingAsync);

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData != null)
            {
                var navigationParameter = navigationData as Dictionary<string, object>;
                Hotel = navigationParameter["hotel"] as Models.Hotel;
                _from = (DateTime)navigationParameter["from"];
                _until = (DateTime)navigationParameter["until"];

                if (Hotel != null)
                {
                    try
                    {
                        IsBusy = true;

                        Hotel = await _hotelService.GetHotelByIdAsync(Hotel.Id);

                        var hotelServices = await _hotelService.GetHotelServicesAsync();
                        HotelServices = hotelServices.ToObservableCollection();

                        var roomServices = await _hotelService.GetRoomServicesAsync();
                        RoomServices = roomServices.ToObservableCollection();

                        var hotelReviews = await _hotelService.GetReviewsAsync(Hotel.Id);
                        HotelReviews = hotelReviews.ToObservableCollection();

                        // Experiment with r https://docs.microsoft.com/en-us/azure/machine-learning/studio/extend-your-experiment-with-r
                        /*
                        if (Hotel.Rooms.Any())
                        {
                            await GetOccupancyAsync();
                        }
                        */
                    }
                    catch (HttpRequestException httpEx)
                    {
                        Debug.WriteLine($"[Booking Hotels Step] Error retrieving data: {httpEx}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[Booking Hotel Step] Error: {ex}");

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

        private void SetMyHotel()
        {
            MyHotel = true;
            Rooms = false;
            Reviews = false;
        }

        private void SetRooms()
        {
            MyHotel = false;
            Rooms = true;
            Reviews = false;
        }

        private void SetReviews()
        {
            MyHotel = false;
            Rooms = false;
            Reviews = true;
        }

        private async Task BookingAsync()
        {
            try
            {
                var authenticatedUser = _authenticationService.AuthenticatedUser;

                var booking = await DialogService.ShowConfirmAsync(
                string.Format(Resources.DialogBookingMessage, Hotel.Name),
                Resources.DialogBookingTitle,
                Resources.DialogYes,
                Resources.DialogNo);

                if (booking)
                {
                    var user = _authenticationService.AuthenticatedUser;

                    var newBooking = new Models.Booking
                    {
                        UserId = user.Email,
                        HotelId = Hotel.Id,
                        Adults = 1,
                        Babies = 0,
                        Kids = 0,
                        Price = Hotel.PricePerNight,
                        Rooms = new List<Models.Room>
                        {
                            new Models.Room { Quantity = 1, RoomType = 0 }
                        },
                        From = _from,
                        To = _until
                    };

                    await _bookingService.CreateBookingAsync(newBooking, authenticatedUser.Token);

                    AppSettings.HasBooking = true;

                    await NavigationService.NavigateToAsync<MainViewModel>();

                    MessagingCenter.Send(newBooking, MessengerKeys.BookingRequested);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Booking] Error: {ex}");

                await DialogService.ShowAlertAsync(
                    Resources.ExceptionMessage,
                    Resources.ExceptionTitle,
                    Resources.DialogOk);
            }
        }

        private async Task GetOccupancyAsync()
        {
            var room = Hotel.Rooms.First();
            var occupancy = await _bookingService.GetOccupancyInformationAsync(room.RoomId, _until);
            var ocuppancyIfSunny = occupancy.OcuppancyIfSunny;

            string toast = string.Empty;

            if (ocuppancyIfSunny <= (100 / 3))
            {
                toast = Resources.LowOccupancy;
            }
            else if (ocuppancyIfSunny > (100 / 3) && ocuppancyIfSunny <= (100 / 1.5))
            {
                toast = Resources.MediumOccupancy;
            }
            else
            {
                toast = Resources.HighOccupancy;
            }

            if (!string.IsNullOrEmpty(toast))
            {
                DialogService.ShowToast(toast);
            }
        }
    }
}