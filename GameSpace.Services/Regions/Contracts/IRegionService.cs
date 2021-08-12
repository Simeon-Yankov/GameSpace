using System.Collections.Generic;

using GameSpace.Data.Models;

namespace GameSpace.Services.Regions.Contracts
{
    public interface IRegionService
    {
        string FormatName(string regionName);

        int GetRegionId(string regionNameFormated);

        string GetRegionName(int regionId);

        IEnumerable<Region> AllRegions();

        Region GetRegion(int regionId);
    }
}