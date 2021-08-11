using System;
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
        public User()
        {
            this.Friends = new HashSet<User>();
            this.GameAccounts = new HashSet<GameAccount>();
            this.Teams = new HashSet<UserTeam>();
        }

        [Required]
        [MaxLength(NicknameMaxLength)]
        public string Nickname { get; set; }

        public DateTime CreatedOn { get; init; }

        [MaxLength(IdMaxLength)]
        [ForeignKey(nameof(ProfileInfo))]
        public string ProfileInfoId { get; init; }

        public virtual ProfileInfo ProfileInfo { get; set; }

        public virtual ICollection<User> Friends { get; init; }

        public virtual ICollection<GameAccount> GameAccounts { get; init; }

        public virtual ICollection<UserTeam> Teams { get; init; }
    }
}