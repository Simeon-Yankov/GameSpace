using System.ComponentModel.DataAnnotations;

using GameSpace.Models.SocialNetworks;

using static GameSpace.Common.GlobalConstants;

namespace GameSpace.Models.Teams
{
    public class EditTeamFromModel
    {
        public int Id { get; init; }

        [Required]
        [StringLength(TeamNameMaxLength, MinimumLength = TeamNameMinLength)]
        public string Name { get; init; }

        [StringLength(MaxDescriptionLength)]
        public string Description { get; init; }

        [Display(Name = "Video Url")]
        public string VideoUrl { get; init; }

        [Display(Name = "Website Url")]
        public string WebsiteUrl { get; init; }

        public int? SocialNetworkId { get; init; }

        public SocialNetworkViewModel SocialNetwork { get; init; }
    }
}