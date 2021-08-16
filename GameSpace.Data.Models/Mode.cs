using System.Collections.Generic;

namespace GameSpace.Data.Models
{
    public class Mode
    {
        public Mode() 
            => this.TeamsTournaments = new HashSet<TeamsTournament>();

        public int Id { get; init; }

        public string Name { get; init; }

        public virtual ICollection<TeamsTournament> TeamsTournaments { get; init; }
    }
}