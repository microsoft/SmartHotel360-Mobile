using MvvmHelpers;
using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Services.Authentication;
using SmartHotel.Clients.Core.Services.OpenUri;
using SmartHotel.Clients.Core.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class MenuViewModel : ViewModelBase, IHandleViewAppearing, IHandleViewDisappearing
    {
        const string skype = "Skype";

        ObservableRangeCollection<Models.MenuItem> menuItems;

        readonly IAuthenticationService authenticationService;
        readonly IOpenUriService openUrlService;

        public MenuViewModel(
            IAuthenticationService authenticationService,
            IOpenUriService openUrlService)
        {
            this.authenticationService = authenticationService;
            this.openUrlService = openUrlService;

            MenuItems = new ObservableRangeCollection<Models.MenuItem>();

            InitMenuItems();
        }

        public string UserName => AppSettings.User?.Name;

        public string UserAvatar => AppSettings.User?.AvatarUrl;

        public ObservableRangeCollection<Models.MenuItem> MenuItems
        {
            get => menuItems;
            set
            {
                menuItems = value;
                OnPropertyChanged();
            }
        }

        public ICommand MenuItemSelectedCommand => new Command<Models.MenuItem>(OnSelectMenuItem);

        public Task OnViewAppearingAsync(VisualElement view)
        {
            MessagingCenter.Subscribe<Booking>(this, MessengerKeys.BookingRequested, OnBookingRequested);
            MessagingCenter.Subscribe<CheckoutViewModel>(this, MessengerKeys.CheckoutRequested, OnCheckoutRequested);

            return Task.FromResult(true);
        }

        public Task OnViewDisappearingAsync(VisualElement view) => Task.FromResult(true);

        void InitMenuItems()
        {
            MenuItems.Add(new Models.MenuItem
            {
                Title = "Home",
                MenuItemType = MenuItemType.Home,
                ViewModelType = typeof(MainViewModel),
                IsEnabled = true
            });

            MenuItems.Add(new Models.MenuItem
            {
                Title = "Book a room",
                MenuItemType = MenuItemType.BookRoom,
                ViewModelType = typeof(BookingViewModel),
                IsEnabled = true
            });

            MenuItems.Add(new Models.MenuItem
            {
                Title = "My Room",
                MenuItemType = MenuItemType.MyRoom,
                ViewModelType = typeof(MyRoomViewModel),
                IsEnabled = AppSettings.HasBooking
            });

            MenuItems.Add(new Models.MenuItem
            {
                Title = "Suggestions",
                MenuItemType = MenuItemType.Suggestions,
                ViewModelType = typeof(SuggestionsViewModel),
                IsEnabled = true
            });

            MenuItems.Add(new Models.MenuItem
            {
                Title = "Concierge",
                MenuItemType = MenuItemType.Concierge,
                IsEnabled = true
            });

            MenuItems.Add(new Models.MenuItem
            {
                Title = "Logout",
                MenuItemType = MenuItemType.Logout,
                ViewModelType = typeof(LoginViewModel),
                IsEnabled = true,
                AfterNavigationAction = RemoveUserCredentials
            });
        }

        async void OnSelectMenuItem(Models.MenuItem item)
        {
            if (item.MenuItemType == MenuItemType.Concierge)
            {
                if (Device.RuntimePlatform == Device.UWP)
                {
                    openUrlService.OpenSkypeBot(AppSettings.SkypeBotId);
                }
                else
                {
                    await OpenBotAsync();
                }
            }
            else if (item.IsEnabled && item.ViewModelType != null)
            {
                item.AfterNavigationAction?.Invoke();
                await NavigationService.NavigateToAsync(item.ViewModelType, item);
            }
        }

        Task RemoveUserCredentials()
        {
            AppSettings.HasBooking = false;

            MessagingCenter.Send(this, MessengerKeys.CheckoutRequested);

            return authenticationService.LogoutAsync();
        }

        void OnBookingRequested(Booking booking)
        {
            if (booking == null)
            {
                return;
            }

            SetMenuItemStatus(MenuItemType.MyRoom, true);
        }

        void OnCheckoutRequested(object args) => SetMenuItemStatus(MenuItemType.MyRoom, false);

        void SetMenuItemStatus(MenuItemType type, bool enabled)
        {
            var menuItem = MenuItems.FirstOrDefault(m => m.MenuItemType == type);

            if (menuItem != null)
            {
                menuItem.IsEnabled = enabled;
            }
        }

        async Task OpenBotAsync()
        {
            await Task.Delay(100);

            var bots = new[] { skype };

            try
            {
                var selectedBot =
                    await DialogService.SelectActionAsync(
                        Resources.BotSelectionMessage,
                        Resources.BotSelectionTitle,
                        bots);

                switch (selectedBot)
                {
                    case skype:
                        openUrlService.OpenSkypeBot(AppSettings.SkypeBotId);
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"OpenBot: {ex}");
            }
        }
    }
}