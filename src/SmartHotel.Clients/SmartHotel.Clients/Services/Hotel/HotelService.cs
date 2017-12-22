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
        private readonly IRequestService _requestService;

        public HotelService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public Task<IEnumerable<City>> GetCitiesAsync()
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("cities");

            string uri = builder.ToString();

            return _requestService.GetAsync<IEnumerable<City>>(uri);
        }

        public Task<IEnumerable<Models.Hotel>> SearchAsync(int cityId)
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("Hotels/search");
            builder.Query = $"cityId={cityId}";

            string uri = builder.ToString();

            return _requestService.GetAsync<IEnumerable<Models.Hotel>>(uri);
        }

        public Task<IEnumerable<Models.Hotel>> SearchAsync(int cityId, int rating, int minPrice, int maxPrice)
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("Hotels/search");
            builder.Query = $"cityId={cityId}&rating={rating}&minPrice={minPrice}&maxPrice={maxPrice}";

            string uri = builder.ToString();

            return _requestService.GetAsync<IEnumerable<Models.Hotel>>(uri);
        }

        public Task<IEnumerable<Models.Hotel>> GetMostVisitedAsync()
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("Hotels/mostVisited");

            string uri = builder.ToString();

            return _requestService.GetAsync<IEnumerable<Models.Hotel>>(uri);
        }

        public Task<Models.Hotel> GetHotelByIdAsync(int id)
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath($"Hotels/{id}");

            string uri = builder.ToString();

            return _requestService.GetAsync<Models.Hotel>(uri);
        }

        public Task<IEnumerable<Review>> GetReviewsAsync(int id)
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath($"Reviews/{id}");

            string uri = builder.ToString();

            return _requestService.GetAsync<IEnumerable<Review>>(uri);
        }

        public Task<IEnumerable<Service>> GetHotelServicesAsync()
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("Services/hotel");

            string uri = builder.ToString();

            return _requestService.GetAsync<IEnumerable<Service>>(uri);
        }

        public Task<IEnumerable<Service>> GetRoomServicesAsync()
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("Services/room");

            string uri = builder.ToString();

            return _requestService.GetAsync<IEnumerable<Service>>(uri);
        }
    }
}