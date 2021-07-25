using System;
using System.Linq;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.Teams.Contracts;

namespace GameSpace.Services.Teams
{
    public class TeamService : ITeamService
    {
        private readonly GameSpaceDbContext data;

        public TeamService(GameSpaceDbContext data)
        {
            this.data = data;
        }

        public bool NameExcists(string name)
            => this.data.Teams.Any(t => t.Name == name);

        public async Task<int> Create(string name, string ownerId)
        {
            var teamData = new Team
            {
                Name = name,
                CreatedOn = DateTime.UtcNow,
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
    }
}