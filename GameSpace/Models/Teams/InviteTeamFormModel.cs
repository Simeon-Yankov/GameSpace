using System.ComponentModel.DataAnnotations;

using static GameSpace.Common.GlobalConstants.User;

namespace GameSpace.Models.Teams
{
    public class InviteTeamFormModel
    {
        public int TeamId { get; init; }

        [StringLength(NicknameMaxLength, MinimumLength = NicknameMinLength)]
        public string Nickname { get; init; }
    }
}