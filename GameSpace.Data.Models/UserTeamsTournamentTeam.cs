using System.ComponentModel.DataAnnotations.Schema;

namespace GameSpace.Data.Models
{
    public class UserTeamsTournamentTeam
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; init; }

        public virtual User User { get; init; }

        [ForeignKey(nameof(TeamsTournamentTeam))]
        public int TeamsTournamentTeamId { get; init; }

        public virtual TeamsTournamentTeam TeamsTournamentTeam { get; init; }

        public bool IsChecked { get; set; }
    }
}