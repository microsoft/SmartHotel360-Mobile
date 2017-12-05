using Foundation;
using SmartHotel.Clients.Core.Services.Authentication;
using System.Threading.Tasks;

namespace SmartHotel.Clients.iOS.Services
{
    public class BrowserCookiesService : IBrowserCookiesService
    {
        public Task ClearCookiesAsync()
        {
            NSHttpCookieStorage storage = NSHttpCookieStorage.SharedStorage;

            foreach (NSHttpCookie cookie in storage.Cookies)
            {
                storage.DeleteCookie(cookie);
            }

            return Task.FromResult(true);
        }
    }
}