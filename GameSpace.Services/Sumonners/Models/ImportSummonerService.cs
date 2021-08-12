namespace GameSpace.Services.Sumonners.Models
{
    public class ImportSummonerService
    {
        public string Id { get; set; }

        public string AccountId { get; set; }

        public string PuuId { get; set; }

        public string Name { get; init; }

        public int ProfileIconId { get; set; }

        public double RevisionDate { get; set; }

        public int SummonerLevel { get; set; }

        public ImportSummonerStatusService Status { get; init; }
    }
}