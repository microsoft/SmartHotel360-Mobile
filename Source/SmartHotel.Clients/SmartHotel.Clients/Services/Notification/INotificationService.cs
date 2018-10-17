using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Notification
{
    public interface INotificationService
    {
        Task<IEnumerable<Models.Notification>> GetNotificationsAsync(int seq, string token);

        Task DeleteNotificationAsync(Models.Notification notification);
    }
}