using System.Collections.Generic;
using System.Linq;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.Regions.Contracts;
using GameSpace.Services.Regions.Models;

namespace GameSpace.Services.Regions
{
    public class RegionService : IRegionService
    {
        private readonly GameSpaceDbContext data;


        public RegionService(GameSpaceDbContext data)
            => this.data = data;

        public string FormatName(string regionName)
            => regionName switch
            {
                "NA" => "na1",
                "EUW" => "euw1",
                "EUNE" => "eun1",
                "TR" => "tr1",
                _ => "na1"
            };

        public int GetRegionId(string regionNameFormated) 
            => this.data
                .Regions
                .Where(r => r.Name == regionNameFormated)
                .Select(r => r.Id)
                .FirstOrDefault();

        public string GetRegionName(int regionId)
            => GetRegionQueryable(regionId)
            .Select(r => new
            {
                Name = r.Name
            })
            .FirstOrDefault()
            .Name;

        public Region GetRegion(int regionId) => GetRegionQueryable(regionId).FirstOrDefault();

        public IEnumerable<RegionServiceModel> AllRegions()
            => this.data
                .Regions
                .Select(r => new RegionServiceModel 
                { 
                    Id = r.Id,
                    Name = r.Name
                })
                .ToList();

        public bool RegionExists(int regionId) => this.data.Regions.Any(r => r.Id == regionId);

        private IQueryable<Region> GetRegionQueryable(int regionId)
            => this.data
            .Regions
            .Where(r => r.Id == regionId)
            .AsQueryable();
    }
}