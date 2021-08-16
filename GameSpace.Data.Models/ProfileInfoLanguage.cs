using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static GameSpace.Common.GlobalConstants;

namespace GameSpace.Data.Models
{
    public class ProfileInfoLanguage
    {
        [MaxLength(IdMaxLength)]
        [ForeignKey(nameof(ProfileInfo))]
        public string ProfileInfoId { get; init; }

        public ProfileInfo ProfileInfo { get; init; }

        [ForeignKey(nameof(Language))]
        public int LanguageId { get; init; }

        public Language Language { get; init; }
    }
}