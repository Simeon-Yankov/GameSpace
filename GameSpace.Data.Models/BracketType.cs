using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameSpace.Data.Models
{
    public class BracketType
    {
        public BracketType() 
            => this.TeamsTournaments = new HashSet<TeamsTournament>();

        [Key]
        public int Id { get; init; }

        public string Name { get; init; }

        public virtual ICollection<TeamsTournament> TeamsTournaments { get; init; }
    }
}