namespace SmartHotel.Clients.Core.Models
{
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public override string ToString()
        {
            return $"{Name}, {Country}";
        }
    }
}
