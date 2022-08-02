using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using GameSpace.Data;
using GameSpace.Infrstructure;
using GameSpace.Models.Messages;
using GameSpace.Services.Messages.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameSpace.Controllers
{
    public class MessageController : Controller
    {
        private readonly GameSpaceDbContext data;
        private readonly IMessageService messages;
        private readonly IMapper mapper;

        public MessageController(GameSpaceDbContext data, IMessageService messages, IMapper mapper)
        {
            this.data = data;
            this.messages = messages;
            this.mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> All()
        {
            //TODO: Friend requests..
            var userId = this.User.Id();

            var list = new List<MessageViewModel>(); //TODO: Not sure!

            var invitationsSend = await this.messages.TeamsInvitationBySenderAsync(userId);

            foreach (var request in invitationsSend)
            {
                var view = this.mapper.Map<MessageViewModel>(request);

                list.Add(view);
            }

            var invitationsRecive = await this.messages.TeamsInvitationByReciverAsync(userId);

            foreach (var request in invitationsRecive)
            {
                var view = this.mapper.Map<MessageViewModel>(request);

                list.Add(view);
            }

            var notifications = await this.messages.GetNotificationsAsync(userId);

            foreach (var notification in notifications)
            {
                var view = this.mapper.Map<MessageViewModel>(notification);

                list.Add(view);
            }

            return View(list.OrderBy(r => r.CreateOn));
        }

        [Authorize]
        public async Task<IActionResult> Clear(int notificationId)
        {
            await this.messages.ClearAsync(notificationId);

            return RedirectToAction(nameof(MessageController.All));
        }
    }
}