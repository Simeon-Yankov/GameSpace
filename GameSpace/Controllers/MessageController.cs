using System.Collections.Generic;
using System.Linq;
using AutoMapper;

using GameSpace.Infrstructure;
using GameSpace.Models.Messages;
using GameSpace.Services.Messages.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace GameSpace.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService messages;
        private readonly IMapper mapper;

        public MessageController(IMessageService messages, IMapper mapper)
        {
            this.messages = messages;
            this.mapper = mapper;
        }

        public IActionResult All()
        {
            //TODO: Friend requests..

            var userId = this.User.Id();

            var invitationsSend = this.messages.TeamsInvitationBySender(userId);
            var invitationsRecive = this.messages.TeamsInvitationByReciver(userId);

            var list = new List<TeamInvitationMessageViewModel>(); //TODO: Not sure!

            foreach (var request in invitationsSend)
            {
                var view = this.mapper.Map<TeamInvitationMessageViewModel>(request);

                list.Add(view);
            }

            foreach (var request in invitationsRecive)
            {
                var view = this.mapper.Map<TeamInvitationMessageViewModel>(request);

                list.Add(view);
            }
            
            return View(list.OrderBy(r => r.CreateOn));
        }
    }
}