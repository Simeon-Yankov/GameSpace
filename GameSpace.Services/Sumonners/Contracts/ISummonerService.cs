using System.Threading.Tasks;

using GameSpace.Services.Sumonners.Models;

namespace GameSpace.Services.Sumonners.Contracts
{
    public interface ISummonerService
    {
        Task<VerifySummonerServiceModel> RandomDefaultIconAsync(string accountId, string regionName, int currentIconId);

        Task Remove(string userId, string accountId);

        Task<byte[]> GetProfileImageAsync(int profileIconId);

        Task<ImportSummonerService> GetJsonInfoBySummonerNameAsync(string nickname, string region);

        Task<ImportSummonerService> GetJsonInfoByAccountIdAsync(string accountId, string regionName);

        Task<SummonerServiceModel> GetAccountByRegionAsync(string userId, int regionId);

        Task<string> GetIdByRegionAsync(string userId, int regionId);

        Task<bool> AccountExistsByRegionIdAsync(string userId, int regionId);

        Task<bool> AccountExistsAsync(string userId, string accountId);

        Task<bool> AlreadyAddedAsync(string summonerName, string regionName, string userId);

        Task<bool> AlreadySummonerWithRegionAsync(string userId, string regionName);

        Task<bool> IsVerifiedByRegionAsync(string userId, int regionId);

        Task AddAsync(string userId, string accountId, string summonerName, int regionId, byte[] profileIcon);

        Task RefreshAsync(string userId, string accountId, string summonerName, byte[] profileIcon);

        Task VerifyAsync(string accountId);
    }
}