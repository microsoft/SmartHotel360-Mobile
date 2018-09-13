using MvvmHelpers;
using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Services.Authentication;
using SmartHotel.Clients.Core.Services.Booking;
using SmartHotel.Clients.Core.Services.Chart;
using SmartHotel.Clients.Core.Services.Notification;
using SmartHotel.Clients.Core.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class HomeViewModel : ViewModelBase, IHandleViewAppearing, IHandleViewDisappearing
    {
        bool hasBooking;
        Microcharts.Chart temperatureChart;
        Microcharts.Chart greenChart;
        ObservableRangeCollection<Notification> notifications;

        readonly INotificationService notificationService;
        readonly IChartService chartService;
        readonly IBookingService bookingService;
        readonly IAuthenticationService authenticationService;

        public HomeViewModel(
            INotificationService notificationService,
            IChartService chartService,
            IBookingService bookingService,
            IAuthenticationService authenticationService)
        {
            this.notificationService = notificationService;
            this.chartService = chartService;
            this.bookingService = bookingService;
            this.authenticationService = authenticationService;
            notifications = new ObservableRangeCollection<Notification>();
        }

        public bool HasBooking
        {
            get => hasBooking;
            set => SetProperty(ref hasBooking, value);
        }

        public Microcharts.Chart TemperatureChart
        {
            get => temperatureChart;
            set => SetProperty(ref temperatureChart, value);
        }

        public Microcharts.Chart GreenChart
        {
            get => greenChart;
            set => SetProperty(ref greenChart, value);
        }

        public ObservableRangeCollection<Notification> Notifications
        {
            get => notifications;
            set => SetProperty(ref notifications, value);
        }

        public ICommand NotificationsCommand => new AsyncCommand(OnNotificationsAsync);

        public ICommand OpenDoorCommand => new AsyncCommand(OpenDoorAsync);

        public ICommand BookRoomCommand => new AsyncCommand(BookRoomAsync);

        public ICommand SuggestionsCommand => new AsyncCommand(SuggestionsAsync);

        public ICommand BookConferenceCommand => new AsyncCommand(BookConferenceAsync);

        public ICommand GoMyRoomCommand => new AsyncCommand(GoMyRoomAsync);

        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                IsBusy = true;

                HasBooking = AppSettings.HasBooking;

                TemperatureChart = await chartService.GetTemperatureChartAsync();
                GreenChart = await chartService.GetGreenChartAsync();

                var authenticatedUser = authenticationService.AuthenticatedUser;
                var notifications = await notificationService.GetNotificationsAsync(3, authenticatedUser.Token);
                Notifications = new ObservableRangeCollection<Models.Notification>(notifications);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Home] Error: {ex}");
                await DialogService.ShowAlertAsync(Resources.ExceptionMessage, Resources.ExceptionTitle, Resources.DialogOk);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Task OnViewAppearingAsync(VisualElement view)
        {
            MessagingCenter.Subscribe<Booking>(this, MessengerKeys.BookingRequested, OnBookingRequested);
            MessagingCenter.Subscribe<CheckoutViewModel>(this, MessengerKeys.CheckoutRequested, OnCheckoutRequested);

            return Task.FromResult(true);
        }

        public Task OnViewDisappearingAsync(VisualElement view) => Task.FromResult(true);

        Task OnNotificationsAsync() => NavigationService.NavigateToAsync(typeof(NotificationsViewModel), Notifications);

        Task OpenDoorAsync() => NavigationService.NavigateToPopupAsync<OpenDoorViewModel>(true);

        Task BookRoomAsync() => NavigationService.NavigateToAsync<BookingViewModel>();

        Task SuggestionsAsync() => NavigationService.NavigateToAsync<SuggestionsViewModel>();

        Task BookConferenceAsync() => NavigationService.NavigateToAsync<BookingViewModel>();

        Task GoMyRoomAsync()
        {
            if (HasBooking)
            {
                return NavigationService.NavigateToAsync<MyRoomViewModel>();
            }
            return Task.FromResult(true);
        }

        void OnBookingRequested(Booking booking)
        {
            if (booking == null)
            {
                return;
            }

            HasBooking = true;
        }

        void OnCheckoutRequested(object args) => HasBooking = false;
    }
}