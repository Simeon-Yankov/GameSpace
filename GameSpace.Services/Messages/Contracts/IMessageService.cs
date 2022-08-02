using System.Collections.Generic;
using System.Threading.Tasks;

using GameSpace.Services.Messages.Modles;

namespace GameSpace.Services.Messages.Contracts
{
    public interface IMessageService
    {
        Task ClearAsync(int notificationId);

        Task<IEnumerable<NotificationMessageServiceModel>> GetNotificationsAsync(string userId);

        Task SendNotificationAsync(string recipientId, string message);

        Task<IEnumerable<TeamInvitationMessageServiceModel>> TeamsInvitationByReciverAsync(string userId);

        Task<IEnumerable<TeamInvitationMessageServiceModel>> TeamsInvitationBySenderAsync(string userId);

        Task<TeamInvitationMessageServiceModel> GetAsync(int id);

        Task DeleteAsync(int requestId);

        Task DeleteAllWithGivenTeamNameAsync(string teamName);

        Task<bool> IsRequestSendAsync(string reciverId, string teamName);
    }
}