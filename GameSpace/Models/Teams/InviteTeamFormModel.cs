using System.ComponentModel.DataAnnotations;

using static GameSpace.Common.GlobalConstants;

namespace GameSpace.Models.Teams
{
    public class InviteTeamFormModel
    {
        public int TeamId { get; init; }

        [StringLength(MaxUsernameLength, MinimumLength = MinUsernameLength)]
        public string Username { get; init; }
    }
}