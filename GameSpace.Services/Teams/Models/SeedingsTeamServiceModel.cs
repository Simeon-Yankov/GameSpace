using System.Collections.Generic;

namespace GameSpace.Services.Teams.Models
{
    public class SeedingsTeamServiceModel
    {
        public SeedingsTeamServiceModel()
        {
            this.SeedMaches = new List<int>();
        }

        public bool IsEliminated { get; set; }

        public List<int> SeedMaches { get; init; }

        public IdNamePairTeamServiceModel teamIdNamePair { get; init; }
    }
}