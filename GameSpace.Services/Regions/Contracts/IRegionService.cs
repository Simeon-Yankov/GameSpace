using System.Collections.Generic;

using GameSpace.Data.Models;
using GameSpace.Services.Regions.Models;

namespace GameSpace.Services.Regions.Contracts
{
    public interface IRegionService
    {
        string FormatName(string regionName);

        int GetRegionId(string regionNameFormated);

        string GetRegionName(int regionId);

        bool RegionExists(int regionId);

        IEnumerable<RegionServiceModel> AllRegions();

        Region GetRegion(int regionId);
    }
}