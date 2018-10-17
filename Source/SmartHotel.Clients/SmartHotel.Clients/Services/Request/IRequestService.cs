using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Request
{
    public interface IRequestService
    {
        Task<TResult> GetAsync<TResult>(string uri, string token = "");

        Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "");

        Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "");

        Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "");

        Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data, string token = "");
    }
}