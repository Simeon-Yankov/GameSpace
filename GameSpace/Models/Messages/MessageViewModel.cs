using System;

namespace GameSpace.Models.Messages
{
    public class MessageViewModel
    {
        public int RequestId { get; init; }

        public int NotificationId { get; init; }

        public string SenderId { get; init; }

        public string ReciverId { get; init; }

        public string ReciverUsername { get; init; }

        public string Message { get; init; }

        public string TeamName { get; init; }

        public byte[] TeamImage { get; init; }

        public byte[] SenderImage { get; init; }

        public DateTime CreateOn { get; init; }

        public bool IsDeleted { get; init; }

        public bool IsSender { get; init; }

        public bool IsNotification => this.Message != null;
    }
}