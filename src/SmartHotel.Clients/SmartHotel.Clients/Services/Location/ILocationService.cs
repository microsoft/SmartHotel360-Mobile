using SmartHotel.Clients.Core.Models;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Location
{
    public interface ILocationService
    {
        Task<GeoLocation> GetPositionAsync();
    }
}