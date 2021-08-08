namespace GameSpace.Services.Users.Contracts
{
    public interface IUserService
    {
        string Id(string nickname);

        string GetName(string id);

        bool AnyMessages(string userId);

        bool UserExcistsByNickname(string nickname);

        bool UserExcistsById(string userId);
    }
}