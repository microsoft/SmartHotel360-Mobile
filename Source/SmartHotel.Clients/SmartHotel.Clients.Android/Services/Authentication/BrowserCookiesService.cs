using Android.OS;
using Android.Webkit;
using SmartHotel.Clients.Core.Services.Authentication;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Droid.Services.Authentication
{
    public class BrowserCookiesService : IBrowserCookiesService
    {
        public Task ClearCookiesAsync()
        {
            var context = Android.App.Application.Context;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.LollipopMr1)
            {
                System.Diagnostics.Debug.WriteLine("Clearing cookies for API >= LollipopMr1");
                CookieManager.Instance.RemoveAllCookies(null);
                CookieManager.Instance.Flush();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Clearing cookies for API < LollipopMr1");
#pragma warning disable CS0618 // Type or member is obsolete
                var cookieSyncMngr = CookieSyncManager.CreateInstance(context);
#pragma warning restore CS0618 // Type or member is obsolete
                cookieSyncMngr.StartSync();
                var cookieManager = CookieManager.Instance;
                cookieManager.RemoveAllCookie();
                cookieManager.RemoveSessionCookie();
                cookieSyncMngr.StopSync();
                cookieSyncMngr.Sync();
            }

            return Task.FromResult(true);
        }
    }
}