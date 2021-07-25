using System.Threading.Tasks;

namespace GameSpace.Services.Teams.Contracts
{
    public interface ITeamService
    {
        bool NameExcists(string name);

        Task<int> Create(string name, string ownerId);

        Task AddMember(int teamId, string userId);
    }
}