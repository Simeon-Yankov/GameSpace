using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.Appearances.Contracts;
using GameSpace.Services.Appearances.Models;
using GameSpace.Services.Messages.Contracts;
using GameSpace.Services.Teams.Contracts;
using GameSpace.Services.Teams.Models;
using GameSpace.Services.Users.Models;

using Team = GameSpace.Data.Models.Team;

using static GameSpace.Common.GlobalConstants.Team;
using Microsoft.EntityFrameworkCore;

namespace GameSpace.Services.Teams
{
    public class TeamService : ITeamService
    {
        private readonly GameSpaceDbContext data;
        private readonly IAppearanceService appearances;
        private readonly IMessageService messages;

        public TeamService(GameSpaceDbContext data, IAppearanceService appearances, IMessageService messages)
        {
            this.data = data;
            this.appearances = appearances;
            this.messages = messages;
        }

        public async Task<int> GetId(string name)
        {
            var result = await this.data
            .Teams
            .Where(t => t.Name == name)
            .Select(t => new
            {
                Id = t.Id
            })
            .FirstAsync();

            return result.Id;
        }

        public async Task<string> GetName(int id)
        {
            var result = await this.data
            .Teams
            .Where(t => t.Id == id)
            .Select(t => new
            {
                Name = t.Name
            })
            .FirstAsync();

            return result.Name;
        }

        public async Task<string> GetOwnerId(string teamName)
        {
            var result = await this.data
                .Teams
                .Where(t => t.Name == teamName)
                .Select(t => new
                {
                    OwnerId = t.OwnerId
                })
                .FirstAsync();

            return result.OwnerId;
        }

        public async Task SendInvitation(string senderId, string reciverId, string teamName) //TODO: in mesage controller
        {
            var requestData = new PendingTeamRequest
            {
                SenderId = senderId,
                ReceiverId = reciverId,
                TeamName = teamName,
                CreatedOn = DateTime.UtcNow
            };

            await this.data.PendingTeamsRequests.AddAsync(requestData);
            await this.data.SaveChangesAsync();
        }

        public async Task AddMember(int teamId, string userId)
        {
            var memberRelaion = new UserTeam
            {
                UserId = userId,
                TeamId = teamId
            };

            await this.data.UsersTeams.AddAsync(memberRelaion);
            await this.data.SaveChangesAsync();
        }

        public async Task RemoveMember(int teamId, string userId)
        {
            var memberRelation = this.data
                .UsersTeams
                .First(ut => ut.TeamId == teamId && ut.UserId == userId);

            this.data.UsersTeams.Remove(memberRelation);
            await this.data.SaveChangesAsync();
        }

        public async Task<IEnumerable<TeamServiceModel>> ByOwner(string userId) // Get Owner Teams
            => await this.data
               .Teams
               .Where(t => t.OwnerId == userId)
               .Select(t => new TeamServiceModel
               {
                   Id = t.Id,
                   Name = t.Name,
                   Image = t.Appearance.Image,
                   Banner = t.Appearance.Banner
               })
               .ToListAsync();

        public async Task<IEnumerable<TeamServiceModel>> UserMemberships(string userId)
             => await this.data
                .Teams
                .Where(t => t.Mombers.Any(m => m.UserId == userId))
                .Select(t => new TeamServiceModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Image = t.Appearance.Image,
                    Banner = t.Appearance.Banner
                })
               .ToListAsync();

        public async Task<TeamMembersServiceModel> Members(string currentUserId, int teamId)
            => await this.data
                .Teams
                .Where(t => t.Id == teamId)
                .Select(t => new TeamMembersServiceModel
                {
                    TeamId = t.Id,
                    IsOwner = t.Mombers.Any(m => m.Team.OwnerId == currentUserId),
                    Members = t
                            .Mombers
                            .Select(m => new TeamMemberServiceModel
                            {
                                Id = m.UserId,
                                Nickname = m.User.Nickname,
                                Image = m.User.ProfileInfo.Appearance.Image,
                                IsMemberOwner = m.UserId == m.Team.OwnerId
                            })
                })
                .FirstOrDefaultAsync();

        public async Task<TeamDetailsServiceModel> Details(int teamId, string userId)
            => await this.data
               .Teams
               .Where(t => t.Id == teamId)
               .Select(t => new TeamDetailsServiceModel
               {
                   Id = t.Id,
                   Name = t.Name,
                   CreatedOn = t.CreatedOn.ToString("d"),
                   Owner = t.Mombers
                            .Where(m => m.UserId == t.OwnerId)
                            .Select(m => new UserOwnerServiceModel
                            {
                                Id = m.User.Id,
                                Name = m.User.Nickname,
                                IsOwner = m.User.Id == userId
                            })
                            .First(),
                   Appearance = new AppearanceServiceModel
                   {
                       Image = t.Appearance.Image,
                       Banner = t.Appearance.Banner
                   },
                   Description = t.Description,
                   VideoUrl = t.VideoUrl,
                   WebsiteUrl = t.WebsiteUrl,
               })
               .FirstOrDefaultAsync();

        public async Task PromoteToOwner(int teamId, string userId)
        {
            var teamData = await this.data
                            .Teams
                            .FirstOrDefaultAsync(t => t.Id == teamId);

            teamData.OwnerId = userId;

            await this.data.SaveChangesAsync();
        }

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

        public async Task Edit(int teamId, string name, string description, string videoUrl, string websiteUrl)
        {
            var teamData = this.data.Teams.First(t => t.Id == teamId);

            teamData.Name = name;
            //teamData.Appearance.Image = team.Image;
            teamData.Description = description;
            teamData.VideoUrl = videoUrl;
            teamData.WebsiteUrl = websiteUrl;

            await this.data.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var teamData = this.data.Teams.First(t => t.Id == id);

            var userTeamRelation = await this.data.UsersTeams.Where(ut => ut.TeamId == id).ToArrayAsync();

            this.data.UsersTeams.RemoveRange(userTeamRelation);
            this.data.Teams.Remove(teamData);

            await this.messages.DeleteAllWithGivenTeamName(teamData.Name);
            await this.data.SaveChangesAsync();
        }

        public async Task<bool> Excists(int id)
            => await this.data
            .Teams
            .AnyAsync(t => t.Id == id);

        public async Task<bool> Excists(string name)
            => await this.data
            .Teams
            .AnyAsync(t => t.Name == name);

        public async Task<bool> ExcistsWantedName(string currName, string wantedName)
            => await this.data
                .Teams
                .Where(t => t.Name != currName)
                .AnyAsync(t => t.Name == wantedName);

        public async Task<bool> IsMemberInTeam(int teamId, string userId)
        {
            var result = await this.data
                .Teams
                .Where(t => t.Id == teamId)
                .Select(t => new
                {
                    IsUserAlreadyMember = t.Mombers.Any(m => m.UserId == userId)
                })
                .FirstOrDefaultAsync();

            return result.IsUserAlreadyMember;
        }

        public async Task<bool> IsTeamFull(int teamId)
        {
            var membersCount = await this.data
                .Teams
                .Where(t => t.Id == teamId)
                .Select(t => new //TODO: ANONIMUS
                {
                    MembersCount = t.Mombers.Count,
                })
                .FirstAsync();

            if (membersCount.MembersCount > MaxTeamSize)
            {
                return true;
            }

            return false;
        }
    }
}