using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.Messages.Contracts;
using GameSpace.Services.Messages.Modles;
using GameSpace.Services.Users.Contracts;
using Microsoft.EntityFrameworkCore;

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

        public async Task ClearAsync(int notificationId)
        {
            var notification = await this.data
                 .Notifications
                 .FirstAsync(n => n.Id == notificationId && n.IsDeleted == false);

            notification.IsDeleted = true;

            await this.data.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotificationMessageServiceModel>> GetNotificationsAsync(
            string userId)
            => await this.data
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
                .ToListAsync();

        public async Task SendNotificationAsync(
            string reciverId,
            string message)
        {
            var notificaion = new Notification
            {
                ReceiverId = reciverId,
                Message = message,
                CreatedOn = DateTime.UtcNow
            };

            await this.data.Notifications.AddAsync(notificaion);
            await this.data.SaveChangesAsync();
        }

        public async Task<IEnumerable<TeamInvitationMessageServiceModel>> TeamsInvitationBySenderAsync(
            string userId)
        {
            var teamsInvitation = await this.data
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
                            .ToListAsync();

            foreach (var teamInitation in teamsInvitation)
            {
                teamInitation.ReciverUsername = this.users.GetNickname(teamInitation.ReciverId);
            }

            return teamsInvitation;
        }

        public async Task<IEnumerable<TeamInvitationMessageServiceModel>> TeamsInvitationByReciverAsync(
            string userId)
            => await this.data
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
                .ToListAsync();

        public async Task<TeamInvitationMessageServiceModel> GetAsync(int id)
        {
            var requestData = await this.data
                            .PendingTeamsRequests
                            .Where(request => request.Id == id && request.IsDeleted == false)
                            .Select(request => new TeamInvitationMessageServiceModel
                            {
                                RequestId = request.Id,
                                SenderId = request.SenderId,
                                ReciverId = request.ReceiverId,
                                TeamName = request.TeamName
                            })
                            .FirstOrDefaultAsync();

            if (requestData is not null)
            {
                requestData.ReciverUsername = this.users.GetNickname(requestData.ReciverId);
            }

            return requestData;
        }

        public async Task DeleteAsync(int requestId)
        {
            var requestData = await this.data
                .PendingTeamsRequests
                .FirstOrDefaultAsync(ptr => ptr.Id == requestId);

            requestData.IsDeleted = true;

            await this.data.SaveChangesAsync();
        }

        public async Task DeleteAllWithGivenTeamNameAsync(string teamName)
        {
            var requests = await this.data
                            .PendingTeamsRequests
                            .Where(ptr => ptr.TeamName == teamName)
                            .ToListAsync();

            foreach (var request in requests)
            {
                request.IsDeleted = true;
            }

            await this.data.SaveChangesAsync();
        }

        public Task<bool> IsRequestSendAsync(string reciverId, string teamName)
            => this.data
                .PendingTeamsRequests
                .Where(request => request.IsDeleted == false)
                .AnyAsync(request => request.TeamName == teamName && request.ReceiverId == reciverId);
    }
}