using System.Threading.Tasks;

using GameSpace.Services.Sumonners.Models;

namespace GameSpace.Services.Sumonners.Contracts
{
    public interface ISummonerService
    {
        Task<VerifySummonerServiceModel> RandomDefaultIcon(string accountId, string regionName, int currentIconId);

        Task Remove(string userId, string accountId);

        Task<byte[]> GetProfileImage(int profileIconId);

        Task<ImportSummonerService> GetJsonInfoBySummonerName(string nickname, string region);

        Task<ImportSummonerService> GetJsonInfoByAccountId(string accountId, string regionName);

        SummonerServiceModel GetAccountByRegion(string userId, int regionId);

        string GetIdByRegion(string userId, int regionId);

        bool AccountExistsByRegionId(string userId, int regionId);

        bool AccountExists(string userId, string accountId);

        bool AlreadyAdded(string summonerName, string regionName, string userId);

        bool AlreadySummonerWithRegion(string userId, string regionName);

        bool IsVerifiedByRegion(string userId, int regionId);

        Task Add(string userId, string accountId, string summonerName, int regionId, byte[] profileIcon);

        Task Refresh(string userId, string accountId, string summonerName, byte[] profileIcon);

        Task Verify(string accountId);
    }
}