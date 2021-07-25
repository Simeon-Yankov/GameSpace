using System.ComponentModel.DataAnnotations;

using static GameSpace.Common.GlobalConstants;

namespace GameSpace.Services.Teams.Models
{
    public class AddTeamServiceModel
    {
        [Required]
        [StringLength(MaxUsernameLength, MinimumLength = MinUsernameLength)]
        public string Name { get; init; }
    }
}