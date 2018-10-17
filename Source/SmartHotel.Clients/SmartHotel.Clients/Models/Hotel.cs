using System.Collections.Generic;

namespace SmartHotel.Clients.Core.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public int Rating { get; set; }
        public int Price { get; set; }
        public int PricePerNight { get; set; }
        public string Picture { get; set; }
        public string DefaultPicture { get; set; }
        public string Street { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public List<string> Pictures { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Service> Services { get; set; }
    }
}