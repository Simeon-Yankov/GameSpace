namespace GameSpace.Models.Messages
{
    public class TeamInvitationMessageViewModel
    {
        public int Id { get; init; }

        public string ReciverId { get; init; }

        public string SenderId { get; init; }

        public string TeamName { get; init; }

        public byte[] TeamImage { get; init; }

        public byte[] SenderImage { get; init; }
    }
}