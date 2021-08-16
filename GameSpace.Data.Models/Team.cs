using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static GameSpace.Common.GlobalConstants;

namespace GameSpace.Data.Models
{
    public class Team
    {
        public Team() 
            => this.Mombers = new HashSet<UserTeam>();

        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(TeamNameMaxLength)]
        public string Name { get; set; }

        [MaxLength(IdMaxLength)]
        public string OwnerId { get; set; }

        public DateTime CreatedOn { get; init; }

        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }

        public string VideoUrl { get; set; }

        public string WebsiteUrl { get; set; }

        [ForeignKey(nameof(Appearance))]
        public int? AppearanceId { get; init; }

        public virtual Appearance Appearance { get; init; }

        //[ForeignKey(nameof(GameStats))]
        //public int? GameStatsId { get; init; }

        //public virtual Stat GameStats { get; init; }

        [ForeignKey(nameof(SocialNetwork))]
        public int? SocialNetworkId { get; init; }

        public virtual SocialNetwork SocialNetwork { get; init; }

        public virtual ICollection<UserTeam> Mombers { get; init; }

        public virtual ICollection<TeamsTournamentTeam> Tournaments { get; init; }
    }
}