using System.Threading.Tasks;
using SmartHotel.Clients.Core.Models;

namespace SmartHotel.Clients.Core.Services.Location
{
    public class FakeLocationService : ILocationService
    {
        public async Task<GeoLocation> GetPositionAsync()
        {
            await Task.Delay(500);

            return GeoLocation.Parse(AppSettings.DefaultFallbackMapsLocation);
        }
    }
}