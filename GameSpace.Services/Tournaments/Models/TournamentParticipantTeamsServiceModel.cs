using System.Collections.Generic;

using GameSpace.Services.Teams.Models;

namespace GameSpace.Services.Tournaments.Models
{
    public class TournamentParticipantTeamsServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public IEnumerable<TeamServiceModel> Participants { get; init; }
    }
}