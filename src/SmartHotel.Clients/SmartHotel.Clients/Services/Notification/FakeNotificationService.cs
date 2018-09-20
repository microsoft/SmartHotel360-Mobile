using SmartHotel.Clients.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Notification
{
    public class FakeNotificationService : INotificationService
    {
        public Task<IEnumerable<Models.Notification>> GetNotificationsAsync(int seq, string token)
        {
            var data = new List<Models.Notification>
            {
                new Models.Notification { Text = "Cleaning services have finished refreshing your room.", Type = NotificationType.BeGreen },
                new Models.Notification { Text = "Your wave-up call reminder has been confirmed for 5:30 am.", Type = NotificationType.Room },
                new Models.Notification { Text = "Your conference room has been customized per your online request.", Type = NotificationType.Hotel },
                new Models.Notification { Text = "The SmartHotel360 bar and grill have a reservation set for 8:00 pm for 3 guests.", Type = NotificationType.Other }
            };

            return Task.FromResult(data.AsEnumerable());
        }

        public Task DeleteNotificationAsync(Models.Notification notification)
        {
            return Task.FromResult(false);
        }
    }
}