using System;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Services.OpenUri
{
    public class OpenUriService : IOpenUriService
    {
        public void OpenUri(string uri)
        {
            Device.OpenUri(new Uri(uri));
        }

        public void OpenFacebookBot(string botId)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Device.OpenUri(new Uri($"fb-messenger://user/{botId}"));
            }
            else
            {
                Device.OpenUri(new Uri($"fb-messenger-public://user-thread/{botId}"));
            }
        }

        public void OpenSkypeBot(string botId)
        {
            Device.OpenUri(new Uri(string.Format("skype:28:{0}?chat", botId)));
        }
    }
}
