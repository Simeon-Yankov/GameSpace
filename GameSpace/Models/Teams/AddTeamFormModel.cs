using System.ComponentModel.DataAnnotations;

using static GameSpace.Common.GlobalConstants.User;

namespace GameSpace.Models.Teams
{
    public class AddTeamFormModel
    {
        [Required]
        [StringLength(NicknameMaxLength, MinimumLength = NicknameMinLength)]
        public string Name { get; init; }
    }
}