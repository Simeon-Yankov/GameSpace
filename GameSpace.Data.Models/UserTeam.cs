﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSpace.Data.Models
{
    public class UserTeam
    {
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; init; }

        public virtual User User { get; init; }

        [Required]
        [ForeignKey(nameof(Team))]
        public int TeamId { get; init; }

        public virtual Team Team { get; init; }
    }
}