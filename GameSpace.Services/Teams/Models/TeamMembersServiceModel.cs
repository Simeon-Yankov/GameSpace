using System.Collections.Generic;

namespace GameSpace.Services.Teams.Models
{
    public class TeamMembersServiceModel
    {
        public int TeamId { get; init; }

        public bool IsOwner { get; init; }

        public IEnumerable<TeamMemberServiceModel> Members { get; init; }
    }
}