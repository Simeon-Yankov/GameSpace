using System.ComponentModel.DataAnnotations;

namespace GameSpace.Models.SocialNetworks
{
    public class SocialNetworkViewModel
    {
        [Display(Name = "Facebook Url")]
        public string FacebookUrl { get; init; }

        [Display(Name = "Twitter Url")]
        public string TwitterUrl { get; init; }

        [Display(Name = "Twitch Url")]
        public string TwitchUrl { get; init; }

        [Display(Name = "Youtube Url")]
        public string YoutubeUrl { get; init; }
    }
}