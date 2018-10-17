using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Authentication
{
    public class DefaultBrowserCookiesService : IBrowserCookiesService
    {
        public Task ClearCookiesAsync() => Task.FromResult(true);
    }
}
