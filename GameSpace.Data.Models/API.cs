using System;
using System.ComponentModel.DataAnnotations;

using static GameSpace.Common.GlobalConstants.API;

namespace GameSpace.Data.Models
{
    public class API
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Key { get; init; }

        [Required]
        public string Value { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}