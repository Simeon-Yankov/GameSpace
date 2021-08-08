using System.Linq;

using GameSpace.Data;
using GameSpace.Services.Users.Contracts;

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

        public string GetName(string id)
            => this.data
                .Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    Nickname = u.Nickname
                })
                .First()
                .Nickname;

        public bool AnyMessages(string userId) => this.data.PendingTeamsRequests.Any(request => request.ReciverId == userId); //TODO FOR FRIEND REQUEST AND MOVE TO MESSAGE CONTROLLER

        public bool UserExcistsByNickname(string nickname) => this.data.Users.Any(u => u.Nickname.ToUpper() == nickname.ToUpper());

        public bool UserExcistsById(string userId) => this.data.Users.Any(u => u.Id == userId);
    }
}