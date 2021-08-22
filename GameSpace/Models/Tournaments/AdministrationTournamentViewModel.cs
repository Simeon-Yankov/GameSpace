using System.Collections.Generic;

using GameSpace.Services.Algorithms.Models;
using GameSpace.Services.Teams.Models;
using GameSpace.Services.Tournaments.Models;

namespace GameSpace.Models.Tournaments
{
    public class AdministrationTournamentViewModel
    {
        public bool IsHoster { get; init; }

        public TournamentServiceModel Details { get; init; }

        public IEnumerable<TeamServiceModel> CheckedInTeams { get; init; }

        public List<IdNamePairTeamServiceModel> CheckedInTeamsIdNamePair { get; init; }

        public SingleEliminationSeedsAlgorithmServiceModel TeamSeeds { get; init; }
    }
}