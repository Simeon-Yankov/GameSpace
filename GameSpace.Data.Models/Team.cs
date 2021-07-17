using System;
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

        public DateTime CreatedOn { get; set; }

        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }

        public string VideoUrl { get; set; }

        public string WebsiteUrl { get; set; }

        [MaxLength(IdMaxLength)]
        [ForeignKey(nameof(SocialNetwork))]
        public int? SocialNetworksId { get; init; }

        public virtual SocialNetwork SocialNetwork { get; init; }
    }
}
