using System.Collections.Generic;
using System.Threading.Tasks;

using GameSpace.Services.Teams.Models;

namespace GameSpace.Services.Teams.Contracts
{
    public interface ITeamService
    {
        int GetId(string name);

        string GetName(int id);

        string GetOwnerId(string teamName);

        Task SendInvitation(string senderId, string reciverId, string teamName);

        Task AddMember(int teamId, string userId);

        Task RemoveMember(int teamId, string userId);

        IEnumerable<TeamServiceModel> ByOwner(string userId);

        IEnumerable<TeamServiceModel> UserMemberships(string userId);

        TeamMembersServiceModel Members(string currentUserId, int teamId);

        TeamDetailsServiceModel Details(int id, string userId);

        Task PromoteToOwner(int teamId, string userId);

        Task<int> Create(string name, byte[] image, string ownerId);

        Task Edit(int teamId, string name, string description, string videoUrl, string websiteUrl);

        Task Delete(int id);

        bool Excists(int id);

        bool Excists(string name);

        bool ExcistsWantedName(string currName, string wantedName);

        bool IsMemberInTeam(int teamId, string userId);

        bool IsTeamFull(int teamId);
    }
}