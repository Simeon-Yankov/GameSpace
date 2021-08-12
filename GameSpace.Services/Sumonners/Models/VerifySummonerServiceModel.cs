namespace GameSpace.Services.Sumonners.Models
{
    public class VerifySummonerServiceModel
    {
        public string AccountId { get; init; }

        public string RegionName { get; init; }

        public int IconId { get; init; }

        public byte[] Icon { get; init; }
    }
}