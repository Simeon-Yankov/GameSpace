using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.Appearances;
using GameSpace.Services.Appearances.Contracts;
using GameSpace.Services.Teams.Contracts;
using GameSpace.Services.Teams.Models;

using static GameSpace.Common.GlobalConstants;

namespace GameSpace.Services.Teams
{
    public class TeamService : ITeamService
    {
        private readonly GameSpaceDbContext data;
        private readonly IAppearanceService appearances;

        public TeamService(GameSpaceDbContext data, IAppearanceService appearances)
        {
            this.data = data;
            this.appearances = appearances;
        }

        public bool NameExcists(string name)
            => this.data.Teams.Any(t => t.Name == name);

        public async Task<int> Create(string name, byte[] image, string ownerId)
        {
            var teamData = new Team
            {
                Name = name,
                CreatedOn = DateTime.UtcNow,
                AppearanceId = await appearances.Create(image), //TODO: Implement in Create image adding
                OwnerId = ownerId
            };

            await this.data.Teams.AddAsync(teamData);
            await this.data.SaveChangesAsync();

            return teamData.Id;
        }

        public async Task AddMember(int teamId, string userId)
        {
            var userTeamData = new UserTeam
            {
                UserId = userId,
                TeamId = teamId
            };

            await this.data.UsersTeams.AddAsync(userTeamData);
            await this.data.SaveChangesAsync();
        }

        public IEnumerable<TeamServiceModel> ByUser(string userId)
        {
            return this.data
               .Teams
               .Where(t => t.OwnerId == userId)
               .Select(t => new TeamServiceModel
               {
                   Name = t.Name,
                   Image = t.Appearance.Image,
                   Banner = t.Appearance.Banner
               })
               .ToList();
        }
    }
}