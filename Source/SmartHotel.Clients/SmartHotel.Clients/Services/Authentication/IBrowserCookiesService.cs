using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Authentication
{
    public interface IBrowserCookiesService
    {
        Task ClearCookiesAsync();
    }
}
