using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSpace.Data.Models
{
    public class UserTeam
    {
        [Required]
        public string UserId { get; init; }


        [Required]
        [ForeignKey(nameof(Team))]
        public int TeamId { get; init; }

        public virtual Team Team { get; init; }
    }
}