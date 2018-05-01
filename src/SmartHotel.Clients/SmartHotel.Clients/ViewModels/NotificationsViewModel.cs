using System.Threading.Tasks;
using SmartHotel.Clients.Core.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using SmartHotel.Clients.Core.Services.Notification;
using SmartHotel.Clients.Core.Services.Analytic;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class NotificationsViewModel : ViewModelBase
    {
        private readonly INotificationService _notificationService;
        private readonly IAnalyticService _analyticService;

        private ObservableCollection<Models.Notification> _notifications;
        private bool _hasItems;

        public NotificationsViewModel(
            INotificationService notificationService,
            IAnalyticService analyticService)
        {
            _notificationService = notificationService;
            _analyticService = analyticService;

            HasItems = true;
        }

        public ObservableCollection<Models.Notification> Notifications
        {
            get { return _notifications; }

            set
            {
                _notifications = value;
                OnPropertyChanged();
            }
        }

        public bool HasItems
        {
            get { return _hasItems; }
            set
            {
                _hasItems = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteNotificationCommand => new Command<Models.Notification>(OnDelete);

        public override Task InitializeAsync(object navigationData)
        {
            if(navigationData != null)
            {
                Notifications = (ObservableCollection<Models.Notification>)navigationData;
                HasItems = Notifications.Count > 0;
            }

            return base.InitializeAsync(navigationData);
        }

        private async void OnDelete(Models.Notification notification)
        {
            if (notification != null)
            {
                Notifications.Remove(notification);
                await _notificationService.DeleteNotificationAsync(notification);
                _analyticService.TrackEvent("DeleteNotification");
                HasItems = Notifications.Count > 0;
            }
        }
    }
}