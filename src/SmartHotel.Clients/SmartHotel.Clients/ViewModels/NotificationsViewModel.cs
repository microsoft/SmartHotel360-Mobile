using System.Threading.Tasks;
using SmartHotel.Clients.Core.ViewModels.Base;
using System.Windows.Input;
using Xamarin.Forms;
using SmartHotel.Clients.Core.Services.Notification;
using SmartHotel.Clients.Core.Services.Analytic;
using MvvmHelpers;

namespace SmartHotel.Clients.Core.ViewModels
{
    public class NotificationsViewModel : ViewModelBase
    {
        readonly INotificationService notificationService;
        readonly IAnalyticService analyticService;

        ObservableRangeCollection<Models.Notification> notifications;
        bool hasItems;

        public NotificationsViewModel(
            INotificationService notificationService,
            IAnalyticService analyticService)
        {
            this.notificationService = notificationService;
            this.analyticService = analyticService;

            HasItems = true;
        }

        public ObservableRangeCollection<Models.Notification> Notifications
        {
            get => notifications;
            set => SetProperty(ref notifications, value);
        }

        public bool HasItems
        {
            get => hasItems;
            set => SetProperty(ref hasItems, value);
        }

        public ICommand DeleteNotificationCommand => new Command<Models.Notification>(OnDelete);

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData != null)
            {
                Notifications = (ObservableRangeCollection<Models.Notification>)navigationData;
                HasItems = Notifications.Count > 0;
            }

            return base.InitializeAsync(navigationData);
        }

        async void OnDelete(Models.Notification notification)
        {
            if (notification != null)
            {
                Notifications.Remove(notification);
                await notificationService.DeleteNotificationAsync(notification);
                analyticService.TrackEvent("DeleteNotification");
                HasItems = Notifications.Count > 0;
            }
        }
    }
}