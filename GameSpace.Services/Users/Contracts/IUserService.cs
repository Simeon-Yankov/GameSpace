using System.Threading.Tasks;

using GameSpace.Services.Users.Models;

namespace GameSpace.Services.Users.Contracts
{
    public interface IUserService
    {
        Task<string> Id(string nickname);

        Task<string> GetNicknameAsync(string id);

        Task<UserProfileServiceModel> Profile(string userId);

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

        Task<bool> AnyMessages(string userId);

        Task<bool> ExcistsByIdAsync(string userId);

        Task<bool> ExcistsByNicknameAsync(string nickname);

        Task<bool> ExcistsWantedNameAsync(string currName, string wantedName);
    }
}