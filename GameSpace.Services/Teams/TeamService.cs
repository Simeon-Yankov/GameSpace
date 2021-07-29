using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.Appearances.Contracts;
using GameSpace.Services.Teams.Contracts;
using GameSpace.Services.Teams.Models;

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

        public bool Excists(int id) => data.Teams.Any(t => t.Id == id);

        public bool NameExcists(string name) => this.data.Teams.Any(t => t.Name == name);

        public bool ExcistsWantedName(string currName, string wantedName)
            => this.data
                .Teams
                .Where(t => t.Name != currName)
                .Any(t => t.Name == wantedName);

        public string GetName(int id) => this.data.Teams.First(t => t.Id == id).Name;

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
            => this.data
               .Teams
               .Where(t => t.OwnerId == userId)
               .Select(t => new TeamServiceModel
               {
                   Id = t.Id,
                   Name = t.Name,
                   Image = t.Appearance.Image,
                   Banner = t.Appearance.Banner
               })
               .ToList();

        public TeamDetailsServiceModel Details(int id)
            => data
               .Teams
               .Where(t => t.Id == id)
               .Select(t => new TeamDetailsServiceModel
               {
                   Id = t.Id,
                   Name = t.Name,
                   CreatedOn = t.CreatedOn.ToString("d"),
                   OwnerName = this.data.Users.Where(u => u.Id == t.OwnerId).FirstOrDefault().UserName,
                   Image = t.Appearance.Image,
                   Banner = t.Appearance.Banner,
                   Description = t.Description,
                   VideoUrl = t.VideoUrl,
                   WebsiteUrl = t.WebsiteUrl,
               })
               .FirstOrDefault();


        public async Task Edit(TeamDetailsServiceModel team)
        {
            var teamData = this.data.Teams.First(t => t.Id == team.Id);

            teamData.Name = team.Name;
            //teamData.Appearance.Image = team.Image;
            teamData.Description = team.Description;
            teamData.VideoUrl = team.VideoUrl;
            teamData.WebsiteUrl = team.WebsiteUrl;

            await this.data.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var teamData = this.data.Teams.First(t => t.Id == id);

            var userTeamRelation = this.data.UsersTeams.First(ut => ut.TeamId == id);

            this.data.UsersTeams.Remove(userTeamRelation);
            this.data.Teams.Remove(teamData);

            await this.data.SaveChangesAsync();
        }
    }
}