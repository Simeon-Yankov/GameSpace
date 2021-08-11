using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.Appearances.Models;
using GameSpace.Services.SocialNetworks.Models;
using GameSpace.Services.Sumonners.Models;
using GameSpace.Services.Users.Contracts;
using GameSpace.Services.Users.Models;

namespace GameSpace.Services.Users
{
    public class UserService : IUserService
    {
        private readonly GameSpaceDbContext data;

        public UserService(GameSpaceDbContext data)
            => this.data = data;

        public string Id(string nickname) // Performance boost
            => this.data
                .Users
                .Where(u => u.Nickname.ToUpper() == nickname.ToUpper())
                .Select(u => new
                {
                    Id = u.Id
                })
                .First()
                .Id;

        public string GetNickname(string id)
            => this.data
                .Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    Nickname = u.Nickname
                })
                .First()
                .Nickname;

        public UserProfileServiceModel Profile(string userId) 
            => this.data
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
                                    Name = ga.SummonerName,
                                    Icon = ga.Icon,
                                    RegionName = ga.Region.Name
                                })
            })
            .FirstOrDefault();

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
            var userData = this.data.Users.First(u => u.Id == userId);

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

        public bool AnyMessages(string userId) => this.data.PendingTeamsRequests.Any(request => request.ReceiverId == userId); //TODO FOR FRIEND REQUEST AND MOVE TO MESSAGE CONTROLLER

        public bool ExcistsById(string userId) => this.data.Users.Any(u => u.Id == userId);

        public bool ExcistsByNickname(string nickname) => this.data.Users.Any(u => u.Nickname.ToUpper() == nickname.ToUpper());

        public bool ExcistsWantedName(string currName, string wantedName)
            => this.data
                .Users
                .Where(u => u.Nickname != currName)
                .Any(u => u.Nickname == wantedName);
    }
}