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
        readonly IRequestService _requestService;

        public NotificationService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public Task<IEnumerable<Models.Notification>> GetNotificationsAsync(int seq, string token)
        {
            var builder = new UriBuilder(AppSettings.NotificationsEndpoint);
            builder.AppendToPath("notifications");
            builder.Query = $"seq={seq.ToString(CultureInfo.InvariantCulture)}";

            var uri = builder.ToString();

            return _requestService.GetAsync<IEnumerable<Models.Notification>>(uri, token);
        }

        public Task DeleteNotificationAsync(Models.Notification notification) => Task.FromResult(false);
    }
}