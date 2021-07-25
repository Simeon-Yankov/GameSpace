using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static GameSpace.Common.GlobalConstants;

namespace GameSpace.Data.Models
{
    public class Team
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(MaxTeamName)]
        public string Name { get; set; }

        [MaxLength(IdMaxLength)]
        public string OwnerId { get; set; }

        public DateTime CreatedOn { get; init; }

        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }

        public string VideoUrl { get; set; }

        public string WebsiteUrl { get; set; }

        [ForeignKey(nameof(SocialNetwork))]
        public int? SocialNetworksId { get; init; }

        public virtual SocialNetwork SocialNetwork { get; init; }

        public virtual ICollection<UserTeam> Mombers { get; init; }
    }
}