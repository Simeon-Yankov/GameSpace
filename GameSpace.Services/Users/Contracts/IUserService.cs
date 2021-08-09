using System.Threading.Tasks;

using GameSpace.Services.Users.Models;

namespace GameSpace.Services.Users.Contracts
{
    public interface IUserService
    {
        string Id(string nickname);

        string GetNickname(string id);

        UserProfileServiceModel Profile(string userId);

        Task Edit(
            string userId,
            string nickname,
            string Biography,
            byte[] image,
            byte[] banner,
            string youtubeUrl,
            string twitchUrl,
            string twitterUrl,
            string facebookUrl);

        bool AnyMessages(string userId);

        bool ExcistsById(string userId);

        bool ExcistsByNickname(string nickname);

        bool ExcistsWantedName(string currName, string wantedName);
    }
}