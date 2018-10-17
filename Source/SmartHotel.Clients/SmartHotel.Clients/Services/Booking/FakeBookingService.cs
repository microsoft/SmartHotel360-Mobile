using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Booking
{
    public class FakeBookingService : IBookingService
    {
        static List<Models.BookingSummary> bookings = new List<Models.BookingSummary>
        {
            new Models.BookingSummary
            {
                Id = 1,
                UserId = "me@smarthotel.com",
                HotelId = 1,
                From = DateTime.Today.AddDays(-7),
                To = DateTime.Today
            },
            new Models.BookingSummary
            {
                Id = 2,
                UserId = "me@smarthotel.com",
                HotelId = 2,
                From = DateTime.Today.AddDays(-14),
                To = DateTime.Today.AddDays(-7)
            }
        };


        static readonly Models.Occupancy occupancy = new Models.Occupancy
        {
            OcuppancyIfSunny = 79.10,
            OccupancyIfNotSunny = 58.50
        };

        public async Task<IEnumerable<Models.BookingSummary>> GetBookingsAsync(string token = "")
        {
            await Task.Delay(500);

            return bookings;
        }

        public async Task<IEnumerable<Models.BookingSummary>> GetLatestBookingsAsync(string token = "")
        {
            await Task.Delay(500);

            return bookings;
        }

        public async Task<IEnumerable<Models.BookingSummary>> GetBookingsByEmailAsync(string email, string token = "")
        {
            await Task.Delay(500);

            return bookings.Where(b => b.UserId == email);
        }

        public async Task<IEnumerable<Models.BookingSummary>> GetLatestBookingsByEmailAsync(string email, string token = "")
        {
            await Task.Delay(500);

            return bookings.Where(b => b.UserId == email);
        }

        public async Task<Models.Booking> CreateBookingAsync(Models.Booking booking, string token = "")
        {
            await Task.Delay(500);

            bookings.Add(new Models.BookingSummary
            {
                HotelId = booking.HotelId,
                UserId = booking.UserId,
                From = booking.From,
                To = booking.To
            });

            return booking;
        }

        public async Task<Models.Occupancy> GetOccupancyInformationAsync(int roomId, DateTime date)
        {
            await Task.Delay(500);

            return occupancy;
        }
    }
}