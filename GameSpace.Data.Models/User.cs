using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

using static GameSpace.Common.GlobalConstants.User;
using static GameSpace.Common.GlobalConstants;

namespace GameSpace.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(NicknameMaxLength)]
        public string Nickname { get; set; }

        [MaxLength(IdMaxLength)]
        [ForeignKey(nameof(ProfileInfo))]
        public string ProfileInfoId { get; init; }

        public virtual ProfileInfo ProfileInfo { get; init; }

        public virtual ICollection<UserTeam> Teams { get; init; }
    }
}