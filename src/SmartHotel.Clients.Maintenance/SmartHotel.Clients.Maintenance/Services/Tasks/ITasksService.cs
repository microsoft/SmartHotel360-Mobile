using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Maintenance.Services.Tasks
{
    public interface ITasksService
    {
        Task<IEnumerable<Models.Task>> GetTasksAsync();

        Task MarkTaskAsResolvedAsync(int taskId);
    }
}
