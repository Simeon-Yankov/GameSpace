using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using GameSpace.Data.Models.Enums;

using static GameSpace.Common.GlobalConstants;

namespace GameSpace.Data.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        public LanguageName Name { get; init; }

        [MaxLength(IdMaxLength)]
        [ForeignKey(nameof(ProfileInfo))]
        public string ProfileInfoId { get; init; }

        public ProfileInfo ProfileInfo { get; init; }
    }
}