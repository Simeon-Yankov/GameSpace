using System.ComponentModel.DataAnnotations;

using GameSpace.Models.SocialNetworks;
using GameSpace.Services.Appearances.Models;

using static GameSpace.Common.GlobalConstants.ProfileInfo;
using static GameSpace.Common.GlobalConstants.User;

namespace GameSpace.Models.User
{
    public class EditUserFormModel
    {
        public string Id { get; init; }

        [Required]
        [StringLength(NicknameMaxLength, MinimumLength = NicknameMinLength)]
        public string Nickname { get; init; }

        [StringLength(BiographyMaxLength)]
        public string Biography { get; init; }

        public AppearanceServiceModel Appearance { get; init; }

        public SocialNetworkViewModel SocialNetwork { get; init; }
    }
}