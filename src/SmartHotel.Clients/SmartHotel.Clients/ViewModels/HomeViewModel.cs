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
using SmartHotel.Clients.Core.Services.IoT;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class HomeViewModel : ViewModelBase, IHandleViewAppearing, IHandleViewDisappearing
    {
        private bool _hasBooking;
        private Microcharts.Chart _temperatureChart;
        private Microcharts.Chart _greenChart;
        private ObservableCollection<Notification> _notifications;

        private readonly INotificationService _notificationService;
        private readonly IChartService _chartService;
        private readonly IBookingService _bookingService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILiveIoTDataService _liveIoTDataService;

        public HomeViewModel(
            INotificationService notificationService,
            IChartService chartService,
            IBookingService bookingService,
            IAuthenticationService authenticationService,
            ILiveIoTDataService liveIoTDataService)
        {
            _notificationService = notificationService;
            _chartService = chartService;
            _bookingService = bookingService;
            _authenticationService = authenticationService;
            _liveIoTDataService = liveIoTDataService;
            _notifications = new ObservableCollection<Notification>();
        }

        public bool HasBooking
        {
            get { return _hasBooking; }
            set
            {
                _hasBooking = value;
                OnPropertyChanged();
            }
        }

        public Microcharts.Chart TemperatureChart
        {
            get { return _temperatureChart; }

            set
            {
                _temperatureChart = value;
                OnPropertyChanged();
            }
        }

        public Microcharts.Chart GreenChart
        {
            get { return _greenChart; }

            set
            {
                _greenChart = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Notification> Notifications
        {
            get { return _notifications; }

            set
            {
                _notifications = value;
                OnPropertyChanged();
            }
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

                TemperatureChart = await _chartService.GetTemperatureChartAsync();
                GreenChart = await _chartService.GetGreenChartAsync();

                var authenticatedUser = _authenticationService.AuthenticatedUser;
                var notifications = await _notificationService.GetNotificationsAsync(3, authenticatedUser.Token);
                Notifications = new ObservableCollection<Models.Notification>(notifications);
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

        public Task OnViewDisappearingAsync(VisualElement view)
        {
            return Task.FromResult(true);
        }

        private Task OnNotificationsAsync()
        {
            return NavigationService.NavigateToAsync(typeof(NotificationsViewModel), Notifications);
        }

        private Task OpenDoorAsync()
        {
            return NavigationService.NavigateToPopupAsync<OpenDoorViewModel>(true);
        }

        private Task BookRoomAsync()
        {
            return NavigationService.NavigateToAsync<BookingViewModel>();
        }

        private Task SuggestionsAsync()
        {
            return NavigationService.NavigateToAsync<SuggestionsViewModel>();
        }

        private Task BookConferenceAsync()
        {
            return NavigationService.NavigateToAsync<BookingViewModel>();
        }

        private Task GoMyRoomAsync()
        {
            if (HasBooking)
            {
                return NavigationService.NavigateToAsync<MyRoomViewModel>();
            }
            return Task.FromResult(true);
        }

        private void OnBookingRequested(Booking booking)
        {
            if (booking == null)
            {
                return;
            }

            HasBooking = true;
        }

        private void OnCheckoutRequested(object args)
        {
            HasBooking = false;
        }
    }
}