using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.HttpClients.Contracts;
using GameSpace.Services.HttpClients.Models;

namespace GameSpace.Services.HttpClients
{
    public class ClientService : IClientService
    {
        static readonly HttpClient client = new HttpClient();

        private readonly GameSpaceDbContext data;

        public ClientService(GameSpaceDbContext data)
            => this.data = data;

        public APIServiceModel GetAPI(string key)
            => this.data
                .APIs
                .Where(api => api.Key == key)
                .Select(api => new APIServiceModel
                {
                    Key = api.Key,
                    Value = api.Value
                })
                .FirstOrDefault();

        public async Task AddAPI(string key, string value)
        {
            var api = new API
            {
                Key = key,
                Value = value,
                LastUpdate = DateTime.UtcNow
            };

            await this.data.APIs.AddAsync(api);
            await this.data.SaveChangesAsync();
        }

        public async Task UpdateAPI(string key, string value)
        {
            var api = this.data
                .APIs
                .Where(api => api.Key == key)
                .FirstOrDefault();

            api.Value = value;

            await this.data.SaveChangesAsync();
        }

        public async Task<HttpResponseMessage> ReadMessage(string url, string key = null)
        {
            if (key != null)
            {
                var api = GetAPI(key);

                url = url.Replace(key, api.Value);
            }

            return await client.GetAsync(url);
        }

        public async Task<byte[]> ReadByteArray(string url, string key = null)
        {
            if (key != null)
            {
                var api = GetAPI(key);

                url = url.Replace(key, api.Value);
            }

            return await client.GetByteArrayAsync(url);
        }
    }
}