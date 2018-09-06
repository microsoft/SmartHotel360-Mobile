using MvvmHelpers;
using SmartHotel.Clients.Core.Exceptions;
using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Services.Authentication;
using SmartHotel.Clients.Core.Services.Booking;
using SmartHotel.Clients.Core.Services.Chart;
using SmartHotel.Clients.Core.Services.File;
using SmartHotel.Clients.Core.Services.Notification;
using SmartHotel.Clients.Core.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartHotel.Clients.Core.Services.IoT;
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
        readonly IFileService fileService;
		readonly IRoomDevicesDataService roomDevicesDataService;

        public HomeViewModel(
            INotificationService notificationService,
            IChartService chartService,
            IBookingService bookingService,
            IAuthenticationService authenticationService,
            IFileService fileService,
            IRoomDevicesDataService liveIoTDataService)
        {
            this.notificationService = notificationService;
            this.chartService = chartService;
            this.bookingService = bookingService;
            this.authenticationService = authenticationService;
            this.fileService = fileService;
            this.roomDevicesDataService = liveIoTDataService;

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

        const string greetingMessageLastShownFileName = "GreetingMessageLastShownDate.txt";        
        const string greetingMessageEmbeddedResourceName = "SmartHotel.Clients.Core.Resources.GreetingMessage.txt";

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
                Notifications = new ObservableRangeCollection<Notification>(notifications);

                ShowGreetingMessage();
            }
            catch (ConnectivityException cex)
            {
                Debug.WriteLine($"[Home] Connectivity Error: {cex}");
                await DialogService.ShowAlertAsync("There is no Internet conection, try again later.", "Error", "Ok");
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

        void ShowGreetingMessage()
        {
            // Check the last time the greeting message was showed. That date is saved in a File.
            // If the file does not exists, or the date in the file is in the past, don't show the greeting-
            if (fileService.ExistsInLocalAppDataFolder(greetingMessageLastShownFileName))
            {
                var textFromFile = fileService.ReadStringFromLocalAppDataFolder(greetingMessageLastShownFileName);
                long.TryParse(textFromFile, out var lastShownTicks);

                if (lastShownTicks < DateTime.Now.Ticks)
                {
                    return;
                }
            }

            // Show greeting message
            var greetingMessage = fileService.ReadStringFromAssemblyEmbeddedResource(greetingMessageEmbeddedResourceName);
            DialogService.ShowToast(greetingMessage);

            // Save last shown date
            var stringTicks = DateTime.Now.Ticks.ToString();
            fileService.WriteStringToLocalAppDataFolder(greetingMessageLastShownFileName, stringTicks);
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