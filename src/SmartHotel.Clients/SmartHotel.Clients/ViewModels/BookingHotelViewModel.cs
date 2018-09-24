using System.Threading.Tasks;
using SmartHotel.Clients.Core.ViewModels.Base;
using System.Windows.Input;
using Xamarin.Forms;
using SmartHotel.Clients.Core.Services.Hotel;
using System;
using System.Diagnostics;
using System.Net.Http;
using SmartHotel.Clients.Core.Services.Booking;
using SmartHotel.Clients.Core.Services.Authentication;
using System.Collections.Generic;
using System.Linq;
using MvvmHelpers;
using SmartHotel.Clients.Core.Extensions;
using SmartHotel.Clients.Core.Exceptions;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class BookingHotelViewModel : ViewModelBase
    {
        bool myHotel;
        bool rooms;
        bool reviews;
        Models.Hotel hotel;
        DateTime from;
        DateTime until;
        ObservableRangeCollection<Models.Service> hotelServices;
        ObservableRangeCollection<Models.Service> roomServices;
        ObservableRangeCollection<Models.Review> hotelReviews;

        readonly IHotelService hotelService;
        readonly IBookingService bookingService;
        readonly IAuthenticationService authenticationService;

        public BookingHotelViewModel(
            IHotelService hotelService,
            IBookingService bookingService,
            IAuthenticationService authenticationService)
        {
            this.hotelService = hotelService;
            this.bookingService = bookingService;
            this.authenticationService = authenticationService;
         
            SetMyHotel();
        }

        public Models.Hotel Hotel
        {
            get => hotel;
            set => SetProperty(ref hotel, value);
        }

        public ObservableRangeCollection<Models.Service> HotelServices
        {
            get => hotelServices;
            set => SetProperty(ref hotelServices, value);
        }

        public ObservableRangeCollection<Models.Service> RoomServices
        {
            get => roomServices;
            set => SetProperty(ref roomServices, value);
        }

        public ObservableRangeCollection<Models.Review> HotelReviews
        {
            get => hotelReviews;
            set => SetProperty(ref hotelReviews, value);
        }

        public bool MyHotel
        {
            get => myHotel;
            set => SetProperty(ref myHotel, value);
        }

        public bool Rooms
        {
            get => rooms;
            set => SetProperty(ref rooms, value);
        }

        public bool Reviews
        {
            get => reviews;
            set => SetProperty(ref reviews, value);
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
                from = (DateTime)navigationParameter["from"];
                until = (DateTime)navigationParameter["until"];

                if (Hotel != null)
                {
                    try
                    {
                        IsBusy = true;

                        Hotel = await hotelService.GetHotelByIdAsync(Hotel.Id);

                        var hotelServices = await hotelService.GetHotelServicesAsync();
                        HotelServices = hotelServices.ToObservableRangeCollection();

                        var roomServices = await hotelService.GetRoomServicesAsync();
                        RoomServices = roomServices.ToObservableRangeCollection();

                        var hotelReviews = await hotelService.GetReviewsAsync(Hotel.Id);
                        HotelReviews = hotelReviews.ToObservableRangeCollection();
                        HotelReviews = hotelReviews.ToObservableRangeCollection();

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
                    catch (ConnectivityException cex)
                    {
                        Debug.WriteLine($"[Booking Hotels Step] Connectivity Error: {cex}");
                        await DialogService.ShowAlertAsync("There is no Internet conection, try again later.", "Error", "Ok");
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

        void SetMyHotel()
        {
            MyHotel = true;
            Rooms = false;
            Reviews = false;
        }

        void SetRooms()
        {
            MyHotel = false;
            Rooms = true;
            Reviews = false;
        }

        void SetReviews()
        {
            MyHotel = false;
            Rooms = false;
            Reviews = true;
        }

        async Task BookingAsync()
        {
            try
            {
                var authenticatedUser = authenticationService.AuthenticatedUser;

                var booking = await DialogService.ShowConfirmAsync(
                string.Format(Resources.DialogBookingMessage, Hotel.Name),
                Resources.DialogBookingTitle,
                Resources.DialogYes,
                Resources.DialogNo);

                if (booking)
                {
                    var user = authenticationService.AuthenticatedUser;

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
                        From = from,
                        To = until
                    };

                    await bookingService.CreateBookingAsync(newBooking, authenticatedUser.Token);
                                               
                    AppSettings.HasBooking = true;

                    await NavigationService.NavigateToAsync<MainViewModel>();

                    MessagingCenter.Send(newBooking, MessengerKeys.BookingRequested);
                }
            }
            catch (ConnectivityException cex)
            {
                Debug.WriteLine($"[Booking] Connectivity Error: {cex}");
                await DialogService.ShowAlertAsync("There is no Internet conection, try again later.", "Error", "Ok");
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

        async Task GetOccupancyAsync()
        {
            var room = Hotel.Rooms.First();
            var occupancy = await bookingService.GetOccupancyInformationAsync(room.RoomId, until);
            var ocuppancyIfSunny = occupancy.OcuppancyIfSunny;

            var toast = string.Empty;

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