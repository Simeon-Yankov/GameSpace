using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static GameSpace.Common.GlobalConstants.GamingAccount;

namespace GameSpace.Data.Models
{
    public class GameAccount
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(AccountIdMaxLength)]
        public string AccountId { get; init; }

        [Required]
        [MaxLength(SummonerNameMaxLength)]
        public string SummonerName { get; set; }

        [Required]//TODO: ICON required
        public byte[] Icon { get; set; }

        public bool IsVerified { get; set; }

        public Region Region { get; init; }

        public Rank Rank { get; init; }

        public DateTime LastUpdated { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; init; }

        public virtual User User { get; init; }
    }
}