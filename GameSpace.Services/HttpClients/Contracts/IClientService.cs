using System.Net.Http;
using System.Threading.Tasks;

using GameSpace.Services.HttpClients.Models;

namespace GameSpace.Services.HttpClients.Contracts
{
    public interface IClientService
    {
        Task<APIServiceModel> GetAPIAsync(string key);

        Task AddAPIAsync(string key, string value);

        Task UpdateAPIAsync(string key, string value);

        Task<HttpResponseMessage> ReadMessageAsync(string url, string apiKey = null);

        Task<byte[]> ReadByteArrayAsync(string url, string apiKey = null);
    }
}