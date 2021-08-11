using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static GameSpace.Common.GlobalConstants.Language;

namespace GameSpace.Data.Models
{
    public class Language
    {
        public Language() 
            => this.ProfilesInfo = new HashSet<ProfileInfoLanguage>();

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(LanguageMaxLength)]
        public string Name { get; init; }

        public virtual ICollection<ProfileInfoLanguage> ProfilesInfo { get; init; }
    }
}