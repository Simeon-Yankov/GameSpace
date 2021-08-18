using System.Collections.Generic;

using GameSpace.Models.Teams;

namespace GameSpace.Models.Tournaments
{
    public class ParticipationTournamentViewModel
    {
        public int Id { get; init; }

        public IEnumerable<TeamViewModel> Teams { get; init; }
    }
}