using System;
using System.ComponentModel.DataAnnotations;

using static GameSpace.Common.GlobalConstants;

namespace GameSpace.Data.Models
{
    public class PendingTeamRequest
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(IdMaxLength)]
        public string SenderId { get; init; }

        [Required]
        [MaxLength(IdMaxLength)]
        public string ReceiverId { get; init; }

        [Required]
        [MaxLength(TeamNameMaxLength)]
        public string TeamName { get; init; }

        public DateTime CreatedOn { get; init; }

        public bool IsDeleted { get; set; }
    }
}