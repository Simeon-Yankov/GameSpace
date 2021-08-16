using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSpace.Data.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        public int BlueSideId { get; init; }

        public int RedSideId { get; init; }

        public int WinnerId { get; init; }

        public DateTime CreatedOn { get; init; }

        [ForeignKey(nameof(TeamsTournament))]
        public int TeamsTournamentId { get; init; }

        public TeamsTournament TeamsTournament { get; init; }
    }
}