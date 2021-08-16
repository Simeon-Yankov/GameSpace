using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameSpace.Data.Models
{
    public class MaximumTeamsFormat
    {
        public MaximumTeamsFormat() 
            => this.TeamsTournaments = new HashSet<TeamsTournament>();

        [Key]
        public int Id { get; init; }

        public int Capacity { get; init; }

        public virtual ICollection<TeamsTournament> TeamsTournaments { get; init; }
    }
}