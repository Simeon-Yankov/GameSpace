using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static GameSpace.Common.GlobalConstants;

namespace GameSpace.Data.Models
{
    public class ProfileInfo
    {
        public ProfileInfo()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Languages = new HashSet<ProfileInfoLanguage>();
        }

        [Key]
        [MaxLength(IdMaxLength)]
        public string Id { get; init; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Balance { get; set; }

        [MaxLength(BiographyMaxLength)]
        public string Biography { get; set; }

        [ForeignKey(nameof(Appearance))]
        public int? AppearanceId { get; init; }

        public virtual Appearance Appearance { get; set; }

        [ForeignKey(nameof(SocialNetwork))]
        public int? SocialNetworkId { get; init; }

        public virtual SocialNetwork SocialNetwork { get; set; }

        public virtual User User { get; init; }

        public virtual ICollection<ProfileInfoLanguage> Languages { get; init; }
    }
}