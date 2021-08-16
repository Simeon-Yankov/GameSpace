using System.ComponentModel.DataAnnotations.Schema;

namespace GameSpace.Data.Models
{
    public class TeamsTournamentTeam
    {
        [ForeignKey(nameof(Team))]
        public int TeamId { get; init; }

        public virtual Team Team { get; init; }

        [ForeignKey(nameof(TeamsTournament))]
        public int TeamsTournamentId { get; init; }

        public virtual TeamsTournament TeamsTournament { get; init; }

        public bool IsChecked { get; set; }

        public bool IsEliminated { get; set; }
    }
}