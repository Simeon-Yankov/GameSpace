using System.Collections.Generic;
using System.Threading.Tasks;

using GameSpace.Services.Teams.Models;

namespace GameSpace.Services.Teams.Contracts
{
    public interface ITeamService
    {
        int GetId(string name);

        string GetName(int id);

        Task SendInvitation(string senderId, string reciverId, string teamName);

        Task AddMember(int teamId, string userId);

        IEnumerable<TeamServiceModel> ByUser(string userId);

        TeamDetailsServiceModel Details(int id);

        Task<int> Create(string name, byte[] image, string ownerId);

        Task Edit(TeamDetailsServiceModel team);

        Task Delete(int id);

        bool Excists(int id);

        bool NameExcists(string name);

        bool ExcistsWantedName(string currName, string wantedName);

        bool IsMemberInTeam(int teamId, string userId);
    }
}