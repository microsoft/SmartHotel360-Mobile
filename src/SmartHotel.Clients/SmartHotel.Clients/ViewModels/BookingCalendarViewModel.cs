using SmartHotel.Clients.Core.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class BookingCalendarViewModel : ViewModelBase
    {
        private Models.City _city;
        private ObservableCollection<DateTime> _dates;
        private DateTime _from;
        private DateTime _until;
        private bool _isNextEnabled;

        public BookingCalendarViewModel()
        {
            var today = DateTime.Today;

            _dates = new ObservableCollection<DateTime>
            {
                today
            };

            SelectedDate(today);
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

        public ObservableCollection<DateTime> Dates
        {
            get { return _dates; }
            set
            {
                _dates = value;
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

        public bool IsNextEnabled
        {
            get { return _isNextEnabled; }
            set
            {
                _isNextEnabled = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectedDateCommand => new Command((d) => SelectedDate(d));

        public ICommand NextCommand => new AsyncCommand(NextAsync);

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData != null)
            {
                City = navigationData as Models.City;
            }

            return base.InitializeAsync(navigationData);
        }

        private void SelectedDate(object date)
        {
            if (date == null)
                return;

            if (Dates.Any())
            {
                From = Dates.OrderBy(d => d.Day).FirstOrDefault();
                Until = Dates.OrderBy(d => d.Day).LastOrDefault();
                IsNextEnabled = Dates.Any() ? true : false;
            }
        }

        private Task NextAsync()
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "city", City },
                { "from", From },
                { "until", Until },
            };

            return NavigationService.NavigateToAsync<BookingHotelsViewModel>(navigationParameter);
        }
    }
}