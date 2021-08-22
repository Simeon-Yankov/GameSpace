using System;
using System.ComponentModel.DataAnnotations;

using static GameSpace.Common.GlobalConstants.Team;

namespace GameSpace.Data.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; init; }

        [Required]
        public string ReceiverId { get; init; }

        [Required]
        [MaxLength(MaxMessageLength)]
        public string Message { get; init; }

        public DateTime CreatedOn { get; init; }

        public bool IsDeleted { get; set; }
    }
}