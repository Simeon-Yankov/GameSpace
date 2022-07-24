using System.Collections.Generic;
using System.Threading.Tasks;

using GameSpace.Services.Teams.Models;

namespace GameSpace.Services.Teams.Contracts
{
    public interface ITeamService
    {
        Task<int> GetId(string name);

        Task<string> GetName(int id);

        Task<string> GetOwnerId(string teamName);

        Task SendInvitation(string senderId, string reciverId, string teamName);

        Task AddMember(int teamId, string userId);

        Task RemoveMember(int teamId, string userId);

        Task<IEnumerable<TeamServiceModel>> ByOwner(string userId);

        Task<IEnumerable<TeamServiceModel>> UserMemberships(string userId);

        Task<TeamMembersServiceModel> Members(string currentUserId, int teamId);

        Task<TeamDetailsServiceModel> Details(int id, string userId);

        Task PromoteToOwner(int teamId, string userId);

        Task<int> Create(string name, byte[] image, string ownerId);

        Task Edit(int teamId, string name, string description, string videoUrl, string websiteUrl);

        Task Delete(int id);

        Task<bool> Excists(int id);

        Task<bool> Excists(string name);

        Task<bool> ExcistsWantedName(string currName, string wantedName);

        Task<bool> IsMemberInTeam(int teamId, string userId);

        Task<bool> IsTeamFull(int teamId);
    }
}