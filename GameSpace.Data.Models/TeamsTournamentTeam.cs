using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSpace.Data.Models
{
    public class TeamsTournamentTeam
    {
        public TeamsTournamentTeam()
        {
            this.InvitedMembers = new HashSet<UserTeamsTournamentTeam>();
        }

        [Key]
        public int Id { get; init; }

        [ForeignKey(nameof(Team))]
        public int TeamId { get; init; }

        public virtual Team Team { get; init; }

        [ForeignKey(nameof(TeamsTournament))]
        public int TeamsTournamentId { get; init; }

        public virtual TeamsTournament TeamsTournament { get; init; }

        public bool IsChecked { get; set; }

        public bool IsEliminated { get; set; }

        public virtual ICollection<UserTeamsTournamentTeam> InvitedMembers { get; init; }
    }
}