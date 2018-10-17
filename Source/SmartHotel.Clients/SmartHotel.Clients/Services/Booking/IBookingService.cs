using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Booking
{
    public interface IBookingService
    {
        Task<IEnumerable<Models.BookingSummary>> GetBookingsAsync(string token = "");
        Task<IEnumerable<Models.BookingSummary>> GetLatestBookingsAsync(string token = "");
        Task<IEnumerable<Models.BookingSummary>> GetBookingsByEmailAsync(string email, string token = "");
        Task<IEnumerable<Models.BookingSummary>> GetLatestBookingsByEmailAsync(string email, string token = "");
        Task<Models.Booking> CreateBookingAsync(Models.Booking booking, string token = "");
        Task<Models.Occupancy> GetOccupancyInformationAsync(int roomId, DateTime date);
    }
}