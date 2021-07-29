using System.Collections.Generic;
using System.Threading.Tasks;

using GameSpace.Services.Teams.Models;

namespace GameSpace.Services.Teams.Contracts
{
    public interface ITeamService
    {
        bool Excists(int id);

        bool NameExcists(string name);

        bool ExcistsWantedName(string currName, string wantedName);

        string GetName(int id);

        Task<int> Create(string name, byte[] image, string ownerId);

        Task AddMember(int teamId, string userId);

        IEnumerable<TeamServiceModel> ByUser(string userId);

        TeamDetailsServiceModel Details(int id);

        Task Edit(TeamDetailsServiceModel team);

        Task Delete(int id);
    }
}