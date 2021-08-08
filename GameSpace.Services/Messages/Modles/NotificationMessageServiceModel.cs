using System;

namespace GameSpace.Services.Messages.Modles
{
    public class NotificationMessageServiceModel
    {
        public int NotificationId { get; init; }

        public string ReciverId { get; init; }

        public string Message { get; init; }

        public DateTime CreatedOn { get; init; }

        public bool IsDeleted { get; set; }
    }
}