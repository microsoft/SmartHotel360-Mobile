using SmartHotel.Clients.Core.Models;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartHotel.Clients.Core.Services.Geolocator
{
    public interface ILocationService
    {
        Task<Location> GetPositionAsync();
    }
}