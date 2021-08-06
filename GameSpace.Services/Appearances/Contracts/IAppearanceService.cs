using System.Threading.Tasks;

namespace GameSpace.Services.Appearances.Contracts
{
    public interface IAppearanceService
    {
        Task<int> Create(int teamId, byte[] image, byte[] banner = null);
    }
}
