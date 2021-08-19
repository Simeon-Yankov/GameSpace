using System.Collections.Generic;

namespace GameSpace.Services.Teams.Models
{
    public class TeamMembersServiceModel
    {
        public int TeamId { get; init; }

        public bool IsOwner { get; init; }

        public IEnumerable<TeamMemberServiceModel> Members { get; init; }

        public int TournamentId { get; set; }

        public bool IsFirstMemberSelected { get; set; }

        public bool IsSecondMemberSelected { get; set; }

        public bool IsThirdMemberSelected { get; set; }

        public bool IsForthMemberSelected { get; set; }

        public bool IsFifthMemberSelected { get; set; }

        public bool IsSixthMemberSelected { get; set; }
    }
}