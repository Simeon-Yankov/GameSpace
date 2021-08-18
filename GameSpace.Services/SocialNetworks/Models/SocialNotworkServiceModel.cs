using System.ComponentModel.DataAnnotations;

namespace GameSpace.Services.SocialNetworks.Models
{
    public class SocialNotworkServiceModel
    {
        [Url]
        public string FacebookUrl { get; init; }

        [Url]
        public string TwitterUrl { get; init; }

        [Url]
        public string TwitchUrl { get; init; }

        [Url]
        public string YoutubeUrl { get; init; }
    }
}