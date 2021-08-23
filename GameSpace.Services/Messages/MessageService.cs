using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.Messages.Contracts;
using GameSpace.Services.Messages.Modles;
using GameSpace.Services.Users.Contracts;

namespace GameSpace.Services.Messages
{
    public class MessageService : IMessageService
    {
        private readonly GameSpaceDbContext data;
        private readonly IUserService users;

        public MessageService(GameSpaceDbContext data, IUserService users)
        {
            this.data = data;
            this.users = users;
        }

        public async Task Clear(int notificationId)
        {
            var notification = this.data
                 .Notifications
                 .First(n => n.Id == notificationId && n.IsDeleted == false);

            notification.IsDeleted = true;

            await this.data.SaveChangesAsync();
        }

        public IEnumerable<NotificationMessageServiceModel> GetNotifications(string userId)
            => this.data
                .Notifications
                .Where(n => n.ReceiverId == userId && n.IsDeleted == false)
                .Select(n => new NotificationMessageServiceModel
                {
                    NotificationId = n.Id,
                    ReciverId = n.ReceiverId,
                    Message = n.Message,
                    CreatedOn = n.CreatedOn,
                    IsDeleted = n.IsDeleted
                })
                .ToList();

        public async Task SendNotification(string reciverId, string message)
        {
            var notificaion = new Notification
            {
                ReceiverId = reciverId,
                Message = message,
                CreatedOn = DateTime.UtcNow
            };

            this.data.Notifications.Add(notificaion);
            await this.data.SaveChangesAsync();
        }

        public IEnumerable<TeamInvitationMessageServiceModel> TeamsInvitationBySender(string userId)
        {
            var teamsInvitation = this.data
                            .PendingTeamsRequests
                            .Where(request => request.SenderId == userId && request.IsDeleted == false)
                            .Select(request => new TeamInvitationMessageServiceModel
                            {
                                RequestId = request.Id,
                                SenderId = request.SenderId,
                                ReciverId = request.ReceiverId,
                                TeamName = request.TeamName,
                                CreatedOn = request.CreatedOn, // TODO: maybe cast
                                IsSender = true
                            })
                            .OrderByDescending(t => t.CreatedOn)
                            .ToList();

            foreach (var teamInitation in teamsInvitation)
            {
                teamInitation.ReciverUsername = this.users.GetNickname(teamInitation.ReciverId);
            }

            return teamsInvitation;
        }

        public IEnumerable<TeamInvitationMessageServiceModel> TeamsInvitationByReciver(string userId)
            => this.data
                .PendingTeamsRequests
                .Where(request => request.ReceiverId == userId && request.IsDeleted == false)
                .Select(request => new TeamInvitationMessageServiceModel
                {
                    RequestId = request.Id,
                    SenderId = request.SenderId,
                    ReciverId = request.ReceiverId,
                    TeamName = request.TeamName,
                    CreatedOn = request.CreatedOn // TODO: maybe cast
                })
                .OrderByDescending(t => t.CreatedOn)
                .ToList();

        public TeamInvitationMessageServiceModel Get(int id)
        {
            var requestData = this.data
                            .PendingTeamsRequests
                            .Where(request => request.Id == id && request.IsDeleted == false)
                            .Select(request => new TeamInvitationMessageServiceModel
                            {
                                RequestId = request.Id,
                                SenderId = request.SenderId,
                                ReciverId = request.ReceiverId,
                                TeamName = request.TeamName
                            })
                            .FirstOrDefault();

            if (requestData is not null)
            {
                requestData.ReciverUsername = this.users.GetNickname(requestData.ReciverId);
            }

            return requestData;
        }

        public async Task Delete(int requestId)
        {
            var requestData = this.data.PendingTeamsRequests.FirstOrDefault(ptr => ptr.Id == requestId);

            requestData.IsDeleted = true;

            await this.data.SaveChangesAsync();
        }

        public async Task DeleteAllWithGivenTeamName(string teamName)
        {
            var requests = this.data
                            .PendingTeamsRequests
                            .Where(ptr => ptr.TeamName == teamName)
                            .ToList();

            foreach (var request in requests)
            {
                request.IsDeleted = true;
            }

            await this.data.SaveChangesAsync();
        }

        public bool IsRequestSend(string reciverId, string teamName)
            => this.data
                .PendingTeamsRequests
                .Where(request => request.IsDeleted == false)
                .Any(request => request.TeamName == teamName && request.ReceiverId == reciverId);
    }
}