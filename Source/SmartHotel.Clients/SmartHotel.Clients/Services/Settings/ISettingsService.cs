using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Settings
{
    public interface ISettingsService<TRemoteSettingsModel> : IBaseSettingsLoader<TRemoteSettingsModel>
        where TRemoteSettingsModel : class
    {
        string RemoteFileUrl { get; set; }

        Task<TRemoteSettingsModel> LoadSettingsAsync();

        Task PersistRemoteSettingsAsync(TRemoteSettingsModel remote);
    }
}
