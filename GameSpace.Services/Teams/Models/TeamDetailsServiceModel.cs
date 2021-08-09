using GameSpace.Services.Appearances.Models;
using GameSpace.Services.Users.Models;

namespace GameSpace.Services.Teams.Models
{
    public class TeamDetailsServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        //[Display(Name = "Owner Name")]
        public UserOwnerServiceModel Owner { get; init; }

        public string CreatedOn { get; init; }

        public string Description { get; set; }

        public string VideoUrl { get; set; }

        public string WebsiteUrl { get; set; }

        public AppearanceServiceModel Appearance { get; init; }
    }
}