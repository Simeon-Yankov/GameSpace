using System.ComponentModel.DataAnnotations;

namespace GameSpace.Data.Models
{
    public class SocialNetwork
    {
        [Key]
        public int Id { get; init; }

        public string FacebookUrl { get; set; }

        public string TwitterUrl { get; set; }

        public string TwitchUrl { get; set; }

        public string YoutubeUrl { get; set; }

        public int TeamId { get; init; }

        public string ProfileInfoId { get; init; }
    }
}