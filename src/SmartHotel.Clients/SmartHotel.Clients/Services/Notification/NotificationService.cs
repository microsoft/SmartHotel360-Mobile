using SmartHotel.Clients.Core.Extensions;
using SmartHotel.Clients.Core.Services.Request;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IRequestService _requestService;

        public NotificationService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<IEnumerable<Models.Notification>> GetNotificationsAsync(int seq, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.NotificationsEndpoint);
            builder.AppendToPath("notifications");
            builder.Query = $"seq={seq.ToString(CultureInfo.InvariantCulture)}";

            string uri = builder.ToString();

            IEnumerable<Models.Notification> notifications =
                await _requestService.GetAsync<IEnumerable<Models.Notification>>(uri, token);

            return notifications;
        }

        public Task DeleteNotificationAsync(Models.Notification notification)
        {
            return Task.FromResult(false);
        }
    }
}