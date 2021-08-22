using System.Net.Http;
using System.Threading.Tasks;

using GameSpace.Services.HttpClients.Models;

namespace GameSpace.Services.HttpClients.Contracts
{
    public interface IClientService
    {
        APIServiceModel GetAPI(string key);

        Task AddAPI(string key, string value);

        Task UpdateAPI(string key, string value);

        Task<HttpResponseMessage> ReadMessage(string url, string apiKey = null);

        Task<byte[]> ReadByteArray(string url, string apiKey = null);
    }
}