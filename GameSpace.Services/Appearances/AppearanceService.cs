using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.Appearances.Contracts;

namespace GameSpace.Services.Appearances
{
    public class AppearanceService : IAppearanceService
    {
        private readonly GameSpaceDbContext data;

        public AppearanceService(GameSpaceDbContext data)
            => this.data = data;

        public async Task<int> Create(byte[] image, byte[] banner = null)
        {
            var appearanceData = new Appearance
            {
                Image = image,
                Banner = banner
            };

            await this.data.Appearances.AddAsync(appearanceData);
            await this.data.SaveChangesAsync();

            return appearanceData.Id;
        }
    }
}