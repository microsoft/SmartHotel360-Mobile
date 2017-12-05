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

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("cities");

            string uri = builder.ToString();

            IEnumerable<City> cities = await _requestService.GetAsync<IEnumerable<City>>(uri);

            return cities;
        }

        public async Task<IEnumerable<Models.Hotel>> SearchAsync(int cityId)
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("Hotels/search");
            builder.Query = $"cityId={cityId}";

            string uri = builder.ToString();

            IEnumerable<Models.Hotel> hotels = await _requestService.GetAsync<IEnumerable<Models.Hotel>>(uri);

            return hotels;
        }

        public async Task<IEnumerable<Models.Hotel>> SearchAsync(int cityId, int rating, int minPrice, int maxPrice)
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("Hotels/search");
            builder.Query = $"cityId={cityId}&rating={rating}&minPrice={minPrice}&maxPrice={maxPrice}";

            string uri = builder.ToString();

            IEnumerable<Models.Hotel> hotels = await _requestService.GetAsync<IEnumerable<Models.Hotel>>(uri);

            return hotels;
        }

        public async Task<IEnumerable<Models.Hotel>> GetMostVisitedAsync()
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("Hotels/mostVisited");

            string uri = builder.ToString();

            IEnumerable<Models.Hotel> hotels = await _requestService.GetAsync<IEnumerable<Models.Hotel>>(uri);

            return hotels;
        }

        public async Task<Models.Hotel> GetHotelByIdAsync(int id)
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath($"Hotels/{id}");

            string uri = builder.ToString();

            Models.Hotel hotel = await _requestService.GetAsync<Models.Hotel>(uri);

            return hotel;
        }

        public async Task<IEnumerable<Review>> GetReviewsAsync(int id)
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath($"Reviews/{id}");

            string uri = builder.ToString();

            IEnumerable<Review> reviews = await _requestService.GetAsync<IEnumerable<Review>>(uri);

            return reviews;
        }

        public async Task<IEnumerable<Service>> GetHotelServicesAsync()
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("Services/hotel");

            string uri = builder.ToString();

            IEnumerable<Service> services = await _requestService.GetAsync<IEnumerable<Service>>(uri);

            return services;
        }

        public async Task<IEnumerable<Service>> GetRoomServicesAsync()
        {
            UriBuilder builder = new UriBuilder(AppSettings.HotelsEndpoint);
            builder.AppendToPath("Services/room");

            string uri = builder.ToString();

            IEnumerable<Service> services = await _requestService.GetAsync<IEnumerable<Service>>(uri);

            return services;
        }
    }
}