using System.Collections.Generic;

using GameSpace.Services.Algorithms.Models;
using GameSpace.Services.Teams.Models;

namespace GameSpace.Services.Algorithms.Contracts
{
    public interface IAlgorithmService
    {
        SingleEliminationSeedsAlgorithmServiceModel SingleEliminationFirstRoundSeeds(
            List<IdNamePairTeamServiceModel> teamIdNamePairs);

        SingleEliminationSeedsAlgorithmServiceModel SingleEliminationSecondRound(
            SingleEliminationSeedsAlgorithmServiceModel teamIdNamePairs);

        SingleEliminationSeedsAlgorithmServiceModel SingleEliminationThirdRound(
            SingleEliminationSeedsAlgorithmServiceModel teamIdNamePairs);
    }
}