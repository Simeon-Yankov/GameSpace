using System;
using System.Linq;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.Appearances.Models;
using GameSpace.Services.SocialNetworks.Models;
using GameSpace.Services.Sumonners.Models;
using GameSpace.Services.Users.Contracts;
using GameSpace.Services.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace GameSpace.Services.Users
{
    public class UserService : IUserService
    {
        private readonly GameSpaceDbContext data;

        public UserService(GameSpaceDbContext data)
            => this.data = data;

        public async Task<string> Id(string nickname) // Performance boost
            => (await this.data
                .Users
                .Where(u => u.Nickname.ToUpper() == nickname.ToUpper())
                .Select(u => new
                {
                    Id = u.Id
                })
                .FirstAsync())
                .Id;

        public async Task<string> GetNicknameAsync(string id)
            => (await this.data
                .Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    Nickname = u.Nickname
                })
                .FirstAsync())
                .Nickname;

        public async Task<UserProfileServiceModel> Profile(string userId) 
            => await this.data
                .Users
                .Where(u => u.Id == userId)
                .Select(u => new UserProfileServiceModel
                {
                    Id = u.Id,
                    Nickname = u.Nickname,
                    Biography = u.ProfileInfo.Biography,
                    CreatedOn = u.CreatedOn,
                    Appearance = new AppearanceServiceModel
                    {
                        Image = u.ProfileInfo.Appearance.Image,
                        Banner = u.ProfileInfo.Appearance.Banner
                    },
                    SocialNetwork = new SocialNotworkServiceModel
                    {
                        FacebookUrl = u.ProfileInfo.SocialNetwork.FacebookUrl,
                        YoutubeUrl = u.ProfileInfo.SocialNetwork.YoutubeUrl,
                        TwitchUrl = u.ProfileInfo.SocialNetwork.TwitchUrl,
                        TwitterUrl = u.ProfileInfo.SocialNetwork.TwitterUrl,
                    },
                    GameAccounts = u.
                                    GameAccounts
                                    .Select(ga => new SummonerServiceModel
                                    {
                                        Id = ga.Id,
                                        Name = ga.SummonerName,
                                        Icon = ga.Icon,
                                        RegionName = ga.Region.Name,
                                        AccountId = ga.AccountId,
                                        LastUpdate = ga.LastUpdated,
                                        LastUpdateDiff = DateTime.UtcNow.Subtract(ga.LastUpdated),
                                        IsVerified = ga.IsVerified
                                    })
                })
                .FirstOrDefaultAsync();

        public async Task Edit(
            string userId,
            string nickname,
            string Biography,
            byte[] image,
            byte[] banner,
            string youtubeUrl,
            string twitchUrl,
            string twitterUrl,
            string facebookUrl)
        {
            var userData = await this.data.Users.FirstAsync(u => u.Id == userId);

            userData.Nickname = nickname;

            userData.ProfileInfo ??= new ProfileInfo();
            var profileInfo = userData.ProfileInfo;

            profileInfo.Biography = Biography;

            profileInfo.Appearance ??= new Appearance();
            var appearance = profileInfo.Appearance;

            appearance.Image = image;
            appearance.Banner = banner;

            profileInfo.SocialNetwork ??= new SocialNetwork();
            var socialNetwork = profileInfo.SocialNetwork;

            socialNetwork.YoutubeUrl = youtubeUrl;
            socialNetwork.TwitchUrl = twitchUrl;
            socialNetwork.TwitchUrl = twitterUrl;
            socialNetwork.FacebookUrl = facebookUrl;

            await this.data.SaveChangesAsync();
        }

        public async Task<bool> AnyMessages(string userId)
            => await this.data
                .PendingTeamsRequests
                .AnyAsync(request => request.ReceiverId == userId); //TODO FOR FRIEND REQUEST AND MOVE TO MESSAGE CONTROLLER

        public async Task<bool> ExcistsByIdAsync(string userId)
            => await this.data
                .Users
                .AnyAsync(u => u.Id == userId);

        public async Task<bool> ExcistsByNicknameAsync(string nickname)
            => await this.data
                .Users
                .AnyAsync(u => u.Nickname.ToUpper() == nickname.ToUpper());

        public async Task<bool> ExcistsWantedNameAsync(string currName, string wantedName)
            => await this.data
                .Users
                .Where(u => u.Nickname != currName)
                .AnyAsync(u => u.Nickname == wantedName);
    }
}