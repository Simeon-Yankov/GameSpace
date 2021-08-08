using System.ComponentModel.DataAnnotations;

namespace GameSpace.Services.Users.Models
{
    public class OwnerServiceModel
    {
        public string Id { get; init; }

        [Display(Name = "Owner Name")]
        public string Name { get; init; }

        public bool IsOwner { get; init; }
    }
}