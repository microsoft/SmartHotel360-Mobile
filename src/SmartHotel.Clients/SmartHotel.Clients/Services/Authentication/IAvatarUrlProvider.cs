namespace SmartHotel.Clients.Core.Services.Authentication
{
    public interface IAvatarUrlProvider
    {
        string GetAvatarUrl(string email);
    }
}
