namespace GameSpace.Services.Teams.Models
{
    public class TeamMemberServiceModel
    {
        public string Id { get; init; }

        public string Nickname { get; init; }

        public byte[] Image { get; init; }

        public bool IsDefaultImage => Image is null;

        public bool IsMemberOwner { get; init; }
    }
}