using System.ComponentModel.DataAnnotations;

using static GameSpace.Common.GlobalConstants.User;

namespace GameSpace.Services.Teams.Models
{
    public class AddServiceModel
    {
        [Required]
        [StringLength(NicknameMaxLength, MinimumLength = NicknameMinLength)]
        public string Name { get; init; }
    }
}