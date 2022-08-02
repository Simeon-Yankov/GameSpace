using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.Regions.Contracts;
using GameSpace.Services.Regions.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<int> GetRegionIdAsync(string regionNameFormated) 
            => await this.data
                .Regions
                .Where(r => r.Name == regionNameFormated)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

        public async Task<string> GetRegionNameAsync(int regionId)
            => (await this.data
            .Regions
            .Where(r => r.Id == regionId)
            .Select(r => new
            {
                Name = r.Name
            })
            .FirstOrDefaultAsync())
            .Name;

        public async Task<Region> GetRegionAsync(int regionId)
            => await this.data
            .Regions
            .Where(r => r.Id == regionId)
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<RegionServiceModel>> AllRegionsAsync()
            => await this.data
                .Regions
                .Select(r => new RegionServiceModel 
                { 
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();

        public async Task<bool> RegionExistsAsync(int regionId)
            => await this.data
            .Regions
            .AnyAsync(r => r.Id == regionId);
    }
}