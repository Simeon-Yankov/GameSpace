using System.Collections.Generic;

namespace GameSpace.Data.Models
{
    public class Region
    {
        public Region()
        {
            this.TeamsTournaments = new HashSet<TeamsTournament>();
            this.GameAccounts = new HashSet<GameAccount>();
        }

        public int Id { get; init; }

        public string Name { get; init; }

        public virtual ICollection<GameAccount> GameAccounts { get; init; }

        public virtual ICollection<TeamsTournament> TeamsTournaments { get; init; }
    }
}