using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static GameSpace.Common.GlobalConstants.Tournament;

namespace GameSpace.Data.Models
{
    public class TeamsTournament
    {
        public TeamsTournament()
        {
            this.Matches = new HashSet<Match>();
            this.RegisteredTeams = new HashSet<TeamsTournamentTeam>();
        }

        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public string Information { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal PrizePool { get; init; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal TicketPrize { get; init; }

        public DateTime StartsOn { get; init; }

        public int CheckInPeriod { get; init; }

        public int GoToGamePeriod { get; set; }

        public int MinimumTeams { get; init; }

        public bool BronzeMatch { get; init; }

        public bool IsFinished { get; set; }

        public bool IsPending { get; set; }

        public bool IsVerified { get; set; }

        [ForeignKey(nameof(Region))]
        public int RegionId { get; init; }

        public virtual Region Region { get; init; }

        [ForeignKey(nameof(BracketType))]
        public int BracketTypeId { get; init; }

        public virtual BracketType BracketType { get; init; }
        
        [ForeignKey(nameof(Map))]
        public int MapId { get; init; }

        public virtual Map Map { get; init; }

        [ForeignKey(nameof(MaximumTeamsFormat))]
        public int MaximumTeamsId { get; init; }

        public virtual MaximumTeamsFormat MaximumTeamsFormat { get; init; }

        [ForeignKey(nameof(Mode))]
        public int ModeId { get; init; }

        public virtual Mode Mode { get; init; }

        [ForeignKey(nameof(TeamSize))]
        public int TeamSizeId { get; init; }

        public virtual TeamSize TeamSize { get; init; }

        [ForeignKey(nameof(HostTournaments))]
        public int HosterId { get; init; }

        public virtual HostTournaments Hoster { get; init; }

        public virtual ICollection<Match> Matches { get; init; }

        public virtual ICollection<TeamsTournamentTeam> RegisteredTeams { get; init; }
    }
}