using System.Collections.Generic;
using System.Threading.Tasks;

using GameSpace.Services.Teams.Models;

namespace GameSpace.Services.Teams.Contracts
{
    public interface ITeamService
    {
        bool NameExcists(string name);

        Task<int> Create(string name, byte[] image, string ownerId);

        Task AddMember(int teamId, string userId);

        IEnumerable<TeamServiceModel> ByUser(string userId);
    }
}