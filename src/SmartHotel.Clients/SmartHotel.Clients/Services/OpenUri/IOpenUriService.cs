namespace SmartHotel.Clients.Core.Services.OpenUri
{
    public interface IOpenUriService
    {
        void OpenUri(string uri);
        void OpenFacebookBot(string botId);
        void OpenSkypeBot(string botId);
    }
}