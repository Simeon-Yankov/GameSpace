using System;

namespace GameSpace.Services.Sumonners.Models
{
    public class SummonerServiceModel
    {
        public int Id { get; init; }

        public string AccountId { get; init; }

        public string Name { get; init; }

        public string RegionName { get; init; }

        public bool IsVerified { get; init; }

        public DateTime LastUpdate { get; init; }

        public TimeSpan LastUpdateDiff { get; init; }

        public byte[] Icon { get; init; }
    }
}