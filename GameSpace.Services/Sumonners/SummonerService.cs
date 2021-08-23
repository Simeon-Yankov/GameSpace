using System;
using System.Linq;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.HttpClients.Contracts;
using GameSpace.Services.Regions.Contracts;
using GameSpace.Services.Sumonners.Contracts;
using GameSpace.Services.Sumonners.Models;

using Newtonsoft.Json;

namespace GameSpace.Services.Sumonners
{
    public class SummonerService : ISummonerService
    {
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

        public async Task<VerifySummonerServiceModel> RandomDefaultIcon(string accountId, string regionName, int currentIconId)
        {
            const int DefaultMaxIconId = 28;
            const int DefaultMinIconId = 1;

            var random = new Random(); //TODO: not sure

            int rendomDefaultIconId = default;

            do
            {
                rendomDefaultIconId = random.Next(DefaultMinIconId, DefaultMaxIconId);
            }
            while (rendomDefaultIconId == currentIconId);

            var randomDefaultIcon = await GetProfileImage(rendomDefaultIconId);

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
            var gameAccount = this.data
                .GameAccounts
                .Where(ga => ga.AccountId == accountId)
                .FirstOrDefault();

            this.data
                .Users
                .Where(u => u.Id == userId)
                .Select(u => u.GameAccounts.Remove(gameAccount));

            this.data.GameAccounts.Remove(gameAccount);

            await this.data.SaveChangesAsync();
        }

        public async Task<byte[]> GetProfileImage(int profileIconId)
        {
            const string version = "11.14.1";

            var profileIconUrl = $"https://ddragon.leagueoflegends.com/cdn/{version}/img/profileicon/{profileIconId}.png";

            var profileIcon = await this.clients.ReadByteArray(profileIconUrl);

            return profileIcon;
        }

        public async Task<ImportSummonerService> GetJsonInfoBySummonerName(string summonerName, string regionName)
        {                                                                       //TODO: What will happend if request overload
            var regionNameFormated = this.regions.FormatName(regionName);

            var url = $"https://{regionNameFormated}.api.riotgames.com/lol/summoner/v4/summoners/by-name/{summonerName}?api_key={RiotAPI}";

            var response = await clients.ReadMessage(url, RiotAPI);

            var jsonString = await response.Content.ReadAsStringAsync();

            var summoner = JsonConvert.DeserializeObject<ImportSummonerService>(jsonString);

            return summoner;
        }

        public async Task<ImportSummonerService> GetJsonInfoByAccountId(string accountId, string regionName)
        {                                                                       //TODO: What will happend if request overload
            var regionNameFormated = this.regions.FormatName(regionName);

            var url = $"https://{regionNameFormated}.api.riotgames.com/lol/summoner/v4/summoners/by-account/{accountId}?api_key={RiotAPI}";

            var response = await this.clients.ReadMessage(url, RiotAPI);

            var jsonString = await response.Content.ReadAsStringAsync();

            var summoner = JsonConvert.DeserializeObject<ImportSummonerService>(jsonString);

            return summoner;
        }

        public async Task Add(string userId, string accountId, string summonerName, int regionId, byte[] profileIcon)
        {
            var user = GetUserQuery(userId).FirstOrDefault();

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

        public async Task Refresh(string userId, string accountId, string summonerName, byte[] profileIcon)
        {
            var gameAccount = GetUserQuery(userId)
                            .Select(u => u
                                        .GameAccounts
                                        .Where(ga => ga.AccountId == accountId)
                                        .FirstOrDefault())
                            .FirstOrDefault();

            gameAccount.SummonerName = summonerName;
            gameAccount.Icon = profileIcon;
            gameAccount.LastUpdated = DateTime.UtcNow;

            await this.data.SaveChangesAsync();
        }

        public SummonerServiceModel GetAccountByRegion(string userId, int regionId)
            => GetGameAccountByUserAndRegionQuery(userId, regionId)
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
                .FirstOrDefault();

        public string GetIdByRegion(string userId, int regionId)
            => GetGameAccountByUserAndRegionQuery(userId, regionId)
                .Select(ga => ga.AccountId)
                .FirstOrDefault();

        public bool AccountExistsByRegionId(string userId, int regionId)
            => this.data
                .GameAccounts
                .Any(ga => ga.UserId == userId && ga.Region.Id == regionId);

        public bool AccountExists(string userId, string accountId)
            => GetUserQuery(userId)
                .Any(u => u
                        .GameAccounts
                        .Any(ga => ga.AccountId == accountId));

        public bool AlreadyAdded(string summonerName, string regionName, string userId)
            => GetUserQuery(userId)
                .Any(u => u
                        .GameAccounts
                        .Any(ga => ga.SummonerName == summonerName && ga.Region.Name == regionName));

        public bool AlreadySummonerWithRegion(string userId, string regionName)
            => this.data
                .GameAccounts
                .Any(ga => ga.UserId == userId && ga.Region.Name == regionName);

        public bool IsVerifiedByRegion(string userId, int regionId)
            => this.data
                .GameAccounts
                .Any(ga => ga.UserId == userId && ga.Region.Id == regionId && ga.IsVerified == true);

        public async Task Verify(string accountId)
        {
            var account = this.data
                .GameAccounts
                .Where(ga => ga.AccountId == accountId)
                .FirstOrDefault();

            account.IsVerified = true;

            await this.data.SaveChangesAsync();
        }

        private IQueryable<GameAccount> GetGameAccountByUserAndRegionQuery(string userId, int regionId)
            => this.data
                .GameAccounts
                .Where(ga => ga.UserId == userId && ga.Region.Id == regionId)
                .AsQueryable();

        private IQueryable<User> GetUserQuery(string userId)
            => this.data
            .Users
            .Where(u => u.Id == userId)
            .AsQueryable();
    }
}