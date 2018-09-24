using SmartHotel.Clients.Core.Extensions;
using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Services.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Hotel
{
    public class HotelService : IHotelService
    {
        readonly IRequestService requestService;

        public HotelService(IRequestService requestService)
        {
            this.requestService = requestService;
        }

        public Task<IEnumerable<City>> GetCitiesAsync()
        {
            var builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("cities");

            var uri = builder.ToString();

            return requestService.GetAsync<IEnumerable<City>>(uri);
        }

        public Task<IEnumerable<Models.Hotel>> SearchAsync(int cityId)
        {
            var builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("Hotels/search");
            builder.Query = $"cityId={cityId}";

            var uri = builder.ToString();

            return requestService.GetAsync<IEnumerable<Models.Hotel>>(uri);
        }

        public Task<IEnumerable<Models.Hotel>> SearchAsync(int cityId, int rating, int minPrice, int maxPrice)
        {
            var builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("Hotels/search");
            builder.Query = $"cityId={cityId}&rating={rating}&minPrice={minPrice}&maxPrice={maxPrice}";

            var uri = builder.ToString();

            return requestService.GetAsync<IEnumerable<Models.Hotel>>(uri);
        }

        public Task<IEnumerable<Models.Hotel>> GetMostVisitedAsync()
        {
            var builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("Hotels/mostVisited");

            var uri = builder.ToString();

            return requestService.GetAsync<IEnumerable<Models.Hotel>>(uri);
        }

        public Task<Models.Hotel> GetHotelByIdAsync(int id)
        {
            var builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath($"Hotels/{id}");

            var uri = builder.ToString();

            return requestService.GetAsync<Models.Hotel>(uri);
        }

        public Task<IEnumerable<Review>> GetReviewsAsync(int id)
        {
            var builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath($"Reviews/{id}");

            var uri = builder.ToString();

            return requestService.GetAsync<IEnumerable<Review>>(uri);
        }

        public Task<IEnumerable<Service>> GetHotelServicesAsync()
        {
            var builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("Services/hotel");

            var uri = builder.ToString();

            return requestService.GetAsync<IEnumerable<Service>>(uri);
        }

        public Task<IEnumerable<Service>> GetRoomServicesAsync()
        {
            var builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("Services/room");

            var uri = builder.ToString();

            return requestService.GetAsync<IEnumerable<Service>>(uri);
        }
    }
}