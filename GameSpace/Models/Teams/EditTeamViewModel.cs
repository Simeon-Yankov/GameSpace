using System.ComponentModel.DataAnnotations;

using GameSpace.Models.SocialNetworks;

namespace GameSpace.Models.Teams
{
    public class EditTeamViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Description { get; init; }

        [Display(Name = "Video Url")]
        public string VideoUrl { get; init; }

        [Display(Name = "Website Url")]
        public string WebsiteUrl { get; init; }

        public int? SocialNetworkId { get; init; }

        public SocialNetworkViewModel SocialNetwork { get; init; }
    }
}