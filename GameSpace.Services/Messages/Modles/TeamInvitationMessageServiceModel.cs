using System;

namespace GameSpace.Services.Messages.Modles
{
    public class TeamInvitationMessageServiceModel
    {
        public int RequestId { get; init; }

        public string SenderId { get; init; }

        public string ReciverId { get; init; }

        public string ReciverUsername { get; set; }

        public string TeamName { get; init; }

        public DateTime CreatedOn { get; init; }

        public bool IsSender { get; init; }
    }
}