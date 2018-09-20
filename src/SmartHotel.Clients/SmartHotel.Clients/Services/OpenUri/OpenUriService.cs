using System;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Services.OpenUri
{
    public class OpenUriService : IOpenUriService
    {
        public void OpenUri(string uri) => Device.OpenUri(new Uri(uri));

        public void OpenSkypeBot(string botId) => Device.OpenUri(new Uri(string.Format("skype:28:{0}?chat", botId)));
    }
}
