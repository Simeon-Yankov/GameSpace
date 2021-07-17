using System.ComponentModel.DataAnnotations;

using static GameSpace.Common.GlobalConstants;

namespace GameSpace.Models.Teams
{
    public class AddTeamFormModel
    {
        [Required]
        [StringLength(MaxUsernameLength, MinimumLength = MinUsernameLength)]
        public string Name { get; init; }
    }
}