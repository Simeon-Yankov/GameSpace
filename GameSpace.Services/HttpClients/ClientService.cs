using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.HttpClients.Contracts;
using GameSpace.Services.HttpClients.Models;
using Microsoft.EntityFrameworkCore;

namespace GameSpace.Services.HttpClients
{
    public class ClientService : IClientService
    {
        static readonly HttpClient client = new HttpClient();

        private readonly GameSpaceDbContext data;

        public ClientService(GameSpaceDbContext data)
            => this.data = data;

        public async Task<APIServiceModel> GetAPIAsync(string key)
            => await this.data
                .APIs
                .Where(api => api.Key == key)
                .Select(api => new APIServiceModel
                {
                    Key = api.Key,
                    Value = api.Value
                })
                .FirstOrDefaultAsync();

        public async Task AddAPIAsync(string key, string value)
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

        public async Task UpdateAPIAsync(string key, string value)
        {
            var api = await this.data
                .APIs
                .Where(api => api.Key == key)
                .FirstOrDefaultAsync();

            api.Value = value;

            await this.data.SaveChangesAsync();
        }

        public async Task<HttpResponseMessage> ReadMessageAsync(string url, string key = null)
        {
            if (key != null)
            {
                var api = await GetAPIAsync(key);

                url = url.Replace(key, api.Value);
            }

            return await client.GetAsync(url);
        }

        public async Task<byte[]> ReadByteArrayAsync(string url, string key = null)
        {
            if (key != null)
            {
                var api = await GetAPIAsync(key);

                url = url.Replace(key, api.Value);
            }

            return await client.GetByteArrayAsync(url);
        }
    }
}