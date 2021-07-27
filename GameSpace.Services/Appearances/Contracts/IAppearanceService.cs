using System.Threading.Tasks;

namespace GameSpace.Services.Appearances.Contracts
{
    public interface IAppearanceService
    {
        Task<int> Create(byte[] image, byte[] banner = null);
    }
}
