using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Services.Authentication;
using SmartHotel.Clients.Core.Services.OpenUri;
using SmartHotel.Clients.Core.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class MenuViewModel : ViewModelBase, IHandleViewAppearing, IHandleViewDisappearing
    {
        const string Skype = "Skype";
        const string FacebookMessenger = "Facebook Messenger";

        private ObservableCollection<Models.MenuItem> _menuItems;

        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenUriService _openUrlService;

        public MenuViewModel(
            IAuthenticationService authenticationService,
            IOpenUriService openUrlService)
        {
            _authenticationService = authenticationService;
            _openUrlService = openUrlService;

            MenuItems = new ObservableCollection<Models.MenuItem>();

            InitMenuItems();
        }

        public string UserName => AppSettings.User?.Name;

        public string UserAvatar => AppSettings.User?.AvatarUrl;

        public ObservableCollection<Models.MenuItem> MenuItems
        {
            get
            {
                return _menuItems;
            }
            set
            {
                _menuItems = value;
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

        public Task OnViewDisappearingAsync(VisualElement view)
        {
            return Task.FromResult(true);
        }

        private void InitMenuItems()
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

        private async void OnSelectMenuItem(Models.MenuItem item)
        {
            if (item.MenuItemType == MenuItemType.Concierge)
            {
                if (Device.RuntimePlatform == Device.UWP)
                {
                    _openUrlService.OpenSkypeBot(AppSettings.SkypeBotId);
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

        private Task RemoveUserCredentials()
        {
            AppSettings.HasBooking = false;

            MessagingCenter.Send(this, MessengerKeys.CheckoutRequested);

            return _authenticationService.LogoutAsync();
        }

        private void OnBookingRequested(Booking booking)
        {
            if (booking == null)
            {
                return;
            }

            SetMenuItemStatus(MenuItemType.MyRoom, true);
        }

        private void OnCheckoutRequested(object args)
        {
            SetMenuItemStatus(MenuItemType.MyRoom, false);
        }

        private void SetMenuItemStatus(MenuItemType type, bool enabled)
        {
            Models.MenuItem menuItem = MenuItems.FirstOrDefault(m => m.MenuItemType == type);

            if (menuItem != null)
            {
                menuItem.IsEnabled = enabled;
            }
        }

        private async Task OpenBotAsync()
        {
            await Task.Delay(100);

            var bots = new[] { Skype, FacebookMessenger };

            try
            {
                var selectedBot =
                    await DialogService.SelectActionAsync(
                        Resources.BotSelectionMessage,
                        Resources.BotSelectionTitle,
                        bots);

                switch (selectedBot)
                {
                    case Skype:
                        _openUrlService.OpenSkypeBot(AppSettings.SkypeBotId);
                        break;
                    case FacebookMessenger:
                        _openUrlService.OpenFacebookBot(AppSettings.FacebookBotId);
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