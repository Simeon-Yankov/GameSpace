using System;
using System.Linq;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.HttpClients.Contracts;
using GameSpace.Services.Regions.Contracts;
using GameSpace.Services.Sumonners.Contracts;
using GameSpace.Services.Sumonners.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GameSpace.Services.Sumonners
{
    public class SummonerService : ISummonerService
    {
        private static Random random = new Random();

        private readonly IClientService clients;
        private readonly GameSpaceDbContext data;
        private readonly IRegionService regions;

        private const string RiotAPI = "RiotAPI";

        public SummonerService(IClientService clients, GameSpaceDbContext data, IRegionService regions)
        {
            this.clients = clients;
            this.data = data;
            this.regions = regions;
        }

        public async Task<VerifySummonerServiceModel> RandomDefaultIconAsync(
            string accountId,
            string regionName,
            int currentIconId)
        {
            const int DefaultMaxIconId = 28;
            const int DefaultMinIconId = 1;

            int rendomDefaultIconId = default;

            do
            {
                rendomDefaultIconId = random.Next(DefaultMinIconId, DefaultMaxIconId);
            }
            while (rendomDefaultIconId == currentIconId);

            var randomDefaultIcon = await GetProfileImageAsync(rendomDefaultIconId);

            return new VerifySummonerServiceModel
            {
                AccountId = accountId,
                RegionName = regionName,
                IconId = rendomDefaultIconId,
                Icon = randomDefaultIcon
            };
        }

        public async Task Remove(string userId, string accountId)
        {
            var gameAccount = await this.data
                .GameAccounts
                .Where(ga => ga.AccountId == accountId)
                .FirstOrDefaultAsync();

            this.data
                .Users
                .Where(u => u.Id == userId)
                .Select(u => u.GameAccounts.Remove(gameAccount));

            this.data.GameAccounts.Remove(gameAccount);

            await this.data.SaveChangesAsync();
        }

        public async Task<byte[]> GetProfileImageAsync(int profileIconId)
        {
            const string version = "11.14.1";

            var profileIconUrl = $"https://ddragon.leagueoflegends.com/cdn/{version}/img/profileicon/{profileIconId}.png";

            var profileIcon = await this.clients.ReadByteArrayAsync(profileIconUrl);

            return profileIcon;
        }

        public async Task<ImportSummonerService> GetJsonInfoBySummonerNameAsync(
            string summonerName,
            string regionName)
        {  //TODO: What will happend if request overload
            var regionNameFormated = this.regions.FormatName(regionName);

            var url = $"https://{regionNameFormated}.api.riotgames.com/lol/summoner/v4/summoners/by-name/{summonerName}?api_key={RiotAPI}";

            var response = await clients.ReadMessageAsync(url, RiotAPI);

            var jsonString = await response.Content.ReadAsStringAsync();

            var summoner = JsonConvert.DeserializeObject<ImportSummonerService>(jsonString);

            return summoner;
        }

        public async Task<ImportSummonerService> GetJsonInfoByAccountIdAsync(string accountId, string regionName)
        {  //TODO: What will happend if request overload
            var regionNameFormated = this.regions.FormatName(regionName);

            var url = $"https://{regionNameFormated}.api.riotgames.com/lol/summoner/v4/summoners/by-account/{accountId}?api_key={RiotAPI}";

            var response = await this.clients.ReadMessageAsync(url, RiotAPI);

            var jsonString = await response.Content.ReadAsStringAsync();

            var summoner = JsonConvert.DeserializeObject<ImportSummonerService>(jsonString);

            return summoner;
        }

        public async Task AddAsync(
            string userId,
            string accountId,
            string summonerName,
            int regionId,
            byte[] profileIcon)
        {
            var user = await this.data
                .Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var gameAccount = new GameAccount
            {
                AccountId = accountId,
                SummonerName = summonerName,
                Icon = profileIcon,
                Region = this.regions.GetRegion(regionId),
                LastUpdated = DateTime.UtcNow
            };

            //TODO: ADD ICON

            user.GameAccounts.Add(gameAccount);

            await this.data.SaveChangesAsync();
        }

        public async Task RefreshAsync(
            string userId,
            string accountId,
            string summonerName,
            byte[] profileIcon)
        {
            var gameAccount = await this.data
                .Users
                .Where(u => u.Id == userId)
                .Select(u => u
                            .GameAccounts
                            .Where(ga => ga.AccountId == accountId)
                            .FirstOrDefault())
                .FirstOrDefaultAsync();

            gameAccount.SummonerName = summonerName;
            gameAccount.Icon = profileIcon;
            gameAccount.LastUpdated = DateTime.UtcNow;

            await this.data.SaveChangesAsync();
        }

        public async Task<SummonerServiceModel> GetAccountByRegionAsync(string userId, int regionId)
            => await this.data
                .GameAccounts
                .Where(ga => ga.UserId == userId && ga.Region.Id == regionId)
                .Select(ga => new SummonerServiceModel
                {
                    Id = ga.Id,
                    AccountId = ga.AccountId,
                    Icon = ga.Icon,
                    IsVerified = ga.IsVerified,
                    LastUpdate = ga.LastUpdated,
                    Name = ga.SummonerName,
                    RegionName = ga.Region.Name
                })
                .FirstOrDefaultAsync();

        public async Task<string> GetIdByRegionAsync(string userId, int regionId)
            => await this.data
                .GameAccounts
                .Where(ga => ga.UserId == userId && ga.Region.Id == regionId)
                .Select(ga => ga.AccountId)
                .FirstOrDefaultAsync();

        public async Task<bool> AccountExistsByRegionIdAsync(string userId, int regionId)
            => await this.data
                .GameAccounts
                .AnyAsync(ga => ga.UserId == userId && ga.Region.Id == regionId);

        public async Task<bool> AccountExistsAsync(string userId, string accountId)
            => await this.data
                .Users
                .Where(u => u.Id == userId)
                .AnyAsync(u => u
                        .GameAccounts
                        .Any(ga => ga.AccountId == accountId));

        public async Task<bool> AlreadyAddedAsync(string summonerName, string regionName, string userId)
            => await this.data
                .Users
                .Where(u => u.Id == userId)
                .AnyAsync(u => u
                        .GameAccounts
                        .Any(ga => ga.SummonerName == summonerName && ga.Region.Name == regionName));

        public async Task<bool> AlreadySummonerWithRegionAsync(string userId, string regionName)
            => await this.data
                .GameAccounts
                .AnyAsync(ga => ga.UserId == userId && ga.Region.Name == regionName);

        public async Task<bool> IsVerifiedByRegionAsync(string userId, int regionId)
            => await this.data
                .GameAccounts
                .AnyAsync(ga => ga.UserId == userId && ga.Region.Id == regionId && ga.IsVerified == true);

        public async Task VerifyAsync(string accountId)
        {
            var account = await this.data
                .GameAccounts
                .Where(ga => ga.AccountId == accountId)
                .FirstOrDefaultAsync();

            account.IsVerified = true;

            await this.data.SaveChangesAsync();
        }
    }
}