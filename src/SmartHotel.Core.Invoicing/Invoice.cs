using System.Collections.Generic;

namespace SmartHotel.Core.Invoicing
{
    public class Invoice
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Items { get; set; }
        public string HotelName { get; set; }
        public int Total { get; set; }
        public string InvoiceNumber { get; set; }

        public override string ToString() => base.ToString();
    }
}