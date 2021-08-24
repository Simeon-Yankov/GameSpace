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

        public int GetId(string name)
            => this.data
            .Teams
            .Where(t => t.Name == name)
            .Select(t => new
            {
                Id = t.Id
            })
            .First()
            .Id;

        public string GetName(int id)
            => this.data
            .Teams
            .Where(t => t.Id == id)
            .Select(t => new
            {
                Name = t.Name
            })
            .First()
            .Name;

        public string GetOwnerId(string teamName)
            => this.data
            .Teams
            .Where(t => t.Name == teamName)
            .Select(t => new
            {
                OwnerId = t.OwnerId
            })
            .First()
            .OwnerId;

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

        public IEnumerable<TeamServiceModel> ByOwner(string userId) // Get Owner Teams
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

        public IEnumerable<TeamServiceModel> UserMemberships(string userId)
             => this.data
                .Teams
                .Where(t => t.Mombers.Any(m => m.UserId == userId))
                .Select(t => new TeamServiceModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Image = t.Appearance.Image,
                    Banner = t.Appearance.Banner
                })
               .ToList();

        public TeamMembersServiceModel Members(string currentUserId, int teamId)
            => this.data
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
                .FirstOrDefault();

        public TeamDetailsServiceModel Details(int teamId, string userId)
            => data
               .Teams
               .Where(t => t.Id == teamId)
               .Select(t => new TeamDetailsServiceModel
               {
                   Id = t.Id,
                   Name = t.Name,
                   CreatedOn = t.CreatedOn.ToString("d"),
                   Owner = this.data
                                .Users
                                .Where(u => u.Id == t.OwnerId)
                                .Select(u => new UserOwnerServiceModel
                                {
                                    Id = u.Id,
                                    Name = u.Nickname,
                                    IsOwner = u.Id == userId
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
               .FirstOrDefault();

        public async Task PromoteToOwner(int teamId, string userId)
        {
            var teamData = this.data
                            .Teams
                            .FirstOrDefault(t => t.Id == teamId);

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

            var userTeamRelation = this.data.UsersTeams.Where(ut => ut.TeamId == id).ToArray();

            this.data.UsersTeams.RemoveRange(userTeamRelation);
            this.data.Teams.Remove(teamData);

            await this.messages.DeleteAllWithGivenTeamName(teamData.Name);
            await this.data.SaveChangesAsync();
        }

        public bool Excists(int id) => data.Teams.Any(t => t.Id == id);

        public bool Excists(string name) => this.data.Teams.Any(t => t.Name == name);

        public bool ExcistsWantedName(string currName, string wantedName)
            => this.data
                .Teams
                .Where(t => t.Name != currName)
                .Any(t => t.Name == wantedName);

        //public bool IsOwner(int teamId, string userId)
        //    => this.data
        //        .Teams
        //        .Where(t => t.Id == teamId)
        //        .Select(t => new
        //        {
        //            IsUserAlreadyMember = t.Mombers.Any(m => m.UserId == userId)
        //        })
        //        .FirstOrDefault()
        //        .IsUserAlreadyMember;

        public bool IsMemberInTeam(int teamId, string userId)
            => this.data
                .Teams
                .Where(t => t.Id == teamId)
                .Select(t => new
                {
                    IsUserAlreadyMember = t.Mombers.Any(m => m.UserId == userId)
                })
                .FirstOrDefault()
                .IsUserAlreadyMember;

        public bool IsTeamFull(int teamId)
        {
            var membersCount = this.data
                .Teams
                .Where(t => t.Id == teamId)
                .Select(t => new //TODO: ANONIMUS
                {
                    MembersCount = t.Mombers.Count,
                })
                .First()
                .MembersCount;

            if (membersCount > MaxTeamSize)
            {
                return true;
            }

            return false;
        }
    }
}