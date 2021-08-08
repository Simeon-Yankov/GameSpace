using GameSpace.Services.Users.Models;
using System.ComponentModel.DataAnnotations;

namespace GameSpace.Services.Teams.Models
{
    public class TeamDetailsServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        //[Display(Name = "Owner Name")]
        public OwnerServiceModel Owner { get; init; }

        [Display(Name = "Created On")]
        public string CreatedOn { get; init; }

        public string Description { get; set; }

        [Display(Name = "Video Url")]
        public string VideoUrl { get; set; }

        [Display(Name = "Website Url")]
        public string WebsiteUrl { get; set; }

        public byte[] Image { get; init; }

        public byte[] Banner { get; init; }
    }
}