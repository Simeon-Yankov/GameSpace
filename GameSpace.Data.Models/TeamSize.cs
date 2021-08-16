using System.Collections.Generic;

namespace GameSpace.Data.Models
{
    public class TeamSize
    {
        public TeamSize() 
            => this.TeamsTournaments = new HashSet<TeamsTournament>();

        public int Id { get; init; }

        public string Format { get; init; }

        public virtual ICollection<TeamsTournament> TeamsTournaments { get; init; }
    }
}