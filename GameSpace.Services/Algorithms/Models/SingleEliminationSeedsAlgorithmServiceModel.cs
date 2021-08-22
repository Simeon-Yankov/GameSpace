using System.Collections.Generic;

using GameSpace.Services.Teams.Models;

namespace GameSpace.Services.Algorithms.Models
{
    public class SingleEliminationSeedsAlgorithmServiceModel
    {
        public SingleEliminationSeedsAlgorithmServiceModel() 
            => this.TeamsSeeds = new List<SeedingsTeamServiceModel>();

        public int CountOfRounds { get; init; }

        public List<SeedingsTeamServiceModel> TeamsSeeds { get; init; }
    }
}