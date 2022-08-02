using System.Collections.Generic;
using System.Threading.Tasks;

using GameSpace.Data.Models;
using GameSpace.Services.Regions.Models;

namespace GameSpace.Services.Regions.Contracts
{
    public interface IRegionService
    {
        string FormatName(string regionName);

        Task<int> GetRegionIdAsync(string regionNameFormated);

        Task<string> GetRegionNameAsync(int regionId);

        Task<bool> RegionExistsAsync(int regionId);

        Task<IEnumerable<RegionServiceModel>> AllRegionsAsync();

        Task<Region> GetRegionAsync(int regionId);
    }
}