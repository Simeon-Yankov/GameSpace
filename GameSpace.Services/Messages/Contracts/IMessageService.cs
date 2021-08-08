using System.Collections.Generic;
using System.Threading.Tasks;

using GameSpace.Services.Messages.Modles;

namespace GameSpace.Services.Messages.Contracts
{
    public interface IMessageService
    {
        IEnumerable<NotificationMessageServiceModel> GetNotifications(string userId);

        Task SendNotification(string recipientId, string message);

        IEnumerable<TeamInvitationMessageServiceModel> TeamsInvitationByReciver(string userId);

        IEnumerable<TeamInvitationMessageServiceModel> TeamsInvitationBySender(string userId);

        TeamInvitationMessageServiceModel Get(int id);

        Task Delete(int requestId);

        Task DeleteAllWithGivenTeamName(string teamName);

        bool IsRequestSend(string reciverId, string teamName);
    }
}