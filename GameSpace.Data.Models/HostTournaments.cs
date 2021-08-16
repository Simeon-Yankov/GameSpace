using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameSpace.Data.Models
{
    public class HostTournaments
    {
        public HostTournaments()
        {
            this.TeamsTournaments = new HashSet<TeamsTournament>();

            //this.IndividualTournaments = new HashSet<IndividualTournament>();
        }

        [Key]
        public int id { get; init; }

        public virtual User User { get; init; }

        public virtual ICollection<TeamsTournament> TeamsTournaments { get; init; }

        //public virtual ICollection<IndividualTournament> IndividualTournaments { get; init; }
    }
}