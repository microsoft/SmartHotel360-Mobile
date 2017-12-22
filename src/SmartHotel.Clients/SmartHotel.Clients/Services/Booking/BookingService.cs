using SmartHotel.Clients.Core.Extensions;
using SmartHotel.Clients.Core.Services.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Booking
{
    public class BookingService : IBookingService
    {
        private readonly IRequestService _requestService;

        public BookingService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public Task<IEnumerable<Models.BookingSummary>> GetBookingsAsync(string token = "")
        {
            UriBuilder builder = new UriBuilder(AppSettings.BookingEndpoint);
            builder.AppendToPath("Bookings");

            string uri = builder.ToString();

            return _requestService.GetAsync<IEnumerable<Models.BookingSummary>>(uri, token);
        }

        public Task<IEnumerable<Models.BookingSummary>> GetLatestBookingsAsync(string token = "")
        {
            UriBuilder builder = new UriBuilder(AppSettings.BookingEndpoint);
            builder.AppendToPath("Bookings/latest");

            string uri = builder.ToString();

            return _requestService.GetAsync<IEnumerable<Models.BookingSummary>>(uri, token);
        }

        public Task<IEnumerable<Models.BookingSummary>> GetBookingsByEmailAsync(string userId, string token = "")
        {
            if (!string.IsNullOrEmpty(token))
            {
                UriBuilder builder = new UriBuilder(AppSettings.BookingEndpoint);
                builder.AppendToPath($"Bookings/{userId}");

                string uri = builder.ToString();

                return _requestService.GetAsync<IEnumerable<Models.BookingSummary>>(uri, token);
            }
            
            return Task.FromResult<IEnumerable<Models.BookingSummary>>(new List<Models.BookingSummary>());
        }

        public Task<IEnumerable<Models.BookingSummary>> GetLatestBookingsByEmailAsync(string email, string token = "")
        {
            UriBuilder builder = new UriBuilder(AppSettings.BookingEndpoint);
            builder.AppendToPath($"Bookings/latest/{email}");

            string uri = builder.ToString();

            return _requestService.GetAsync<IEnumerable<Models.BookingSummary>>(uri, token);
        }

        public Task<Models.Booking> CreateBookingAsync(Models.Booking booking, string token = "")
        {
            var builder = new UriBuilder(AppSettings.BookingEndpoint);
            builder.AppendToPath("Bookings");

            var uri = builder.ToString();

            return _requestService.PostAsync<Models.Booking>(uri, booking, token);
        }

        public Task<Models.Occupancy> GetOccupancyInformationAsync(int roomId, DateTime date)
        {
            var builder = new UriBuilder(AppSettings.BookingEndpoint);
            builder.AppendToPath($"Rooms/{roomId}/occupancy");
            builder.Query = $"date={date.ToString("MM/dd/yyyy")}";

            var uri = builder.ToString();

            return _requestService.GetAsync<Models.Occupancy>(uri);
        }
    }
}