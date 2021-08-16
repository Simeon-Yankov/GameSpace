using System;

namespace GameSpace.Models.Summoners
{
    public class SummonerQueryModel
    {
        public string UserId { get; init; }

        public string AccountId { get; init; }

        public string RegionName { get; init; }

        public DateTime Timer { get; init; }
    }
}