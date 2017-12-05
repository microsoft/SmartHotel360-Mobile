using Newtonsoft.Json;

namespace SmartHotel.Clients.Maintenance.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Room { get; set; }
        public string Description { get; set; }
        public bool Resolved { get; set; }
        public int TaskType { get; set; }
    }
}