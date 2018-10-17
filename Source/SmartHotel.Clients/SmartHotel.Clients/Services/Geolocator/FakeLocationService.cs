using SmartHotel.Clients.Core.Extensions;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartHotel.Clients.Core.Services.Geolocator
{
    public class FakeLocationService : ILocationService
    {
        public async Task<Location> GetPositionAsync()
        {
            await Task.Delay(500);

            return AppSettings.DefaultFallbackMapsLocation.ParseLocation();
        }
    }
}