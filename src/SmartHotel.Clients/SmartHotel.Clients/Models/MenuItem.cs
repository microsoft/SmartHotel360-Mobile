using SmartHotel.Clients.Core.ViewModels.Base;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Models
{
    public class MenuItem : BindableObject
    {
        private string _title;
        private MenuItemType _menuItemType;
        private Type _viewModelType;
        private bool _isEnabled;

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public MenuItemType MenuItemType
        {
            get
            {
                return _menuItemType;
            }

            set
            {
                _menuItemType = value;
                OnPropertyChanged();
            }
        }

        public Type ViewModelType
        {
            get
            {
                return _viewModelType;
            }

            set
            {
                _viewModelType = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public Func<Task> AfterNavigationAction { get; set; }
    }
}