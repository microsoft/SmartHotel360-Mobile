using System;
using System.Collections.Generic;

namespace SmartHotel.Clients.Core.Models
{
    public class Booking
    {
        public int HotelId { get; set; }
        public string UserId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Adults { get; set; }
        public int Kids { get; set; }
        public int Babies { get; set; }
        public List<Room> Rooms { get; set; }
        public int Price { get; set; }
    }
}