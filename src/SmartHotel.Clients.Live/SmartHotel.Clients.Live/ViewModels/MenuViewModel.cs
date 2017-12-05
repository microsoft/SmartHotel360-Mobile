using SmartHotel.Clients.Live.Models;
using SmartHotel.Clients.Live.ViewModels.Base;
using System.Collections.ObjectModel;

namespace SmartHotel.Clients.Live.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private ObservableCollection<MenuItem> _menuItems = new ObservableCollection<MenuItem>();

        public MenuViewModel()
        {
            InitMenuItems();
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                return _menuItems;
            }
            set
            {
                _menuItems = value;
                RaisePropertyChanged(() => MenuItems);
            }
        }

        private void InitMenuItems()
        {
            MenuItems.Add(new MenuItem
            {
                Title = "Home",
                ViewModelType = typeof(MainViewModel),
                IsEnabled = true
            });

            MenuItems.Add(new MenuItem
            {
                Title = "Book a room",
                IsEnabled = false
            });

            MenuItems.Add(new MenuItem
            {
                Title = "My Room",
                IsEnabled = false
            });

            MenuItems.Add(new MenuItem
            {
                Title = "Suggestions",
                IsEnabled = false
            });

            MenuItems.Add(new MenuItem
            {
                Title = "Concierge",
                IsEnabled = false
            });

            MenuItems.Add(new MenuItem
            {
                Title = "Logout",
                ViewModelType = typeof(LoginViewModel),
                IsEnabled = true
            });
        }
    }
}