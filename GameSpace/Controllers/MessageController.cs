using System.Collections.Generic;

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
            var invitationRequest = this.messages.TeamsInvitationByReciver(this.User.Id());

            var list = new List<TeamInvitationMessageViewModel>(); //TODO: Not sure!

            foreach (var request in invitationRequest)
            {
                var view = this.mapper.Map<TeamInvitationMessageViewModel>(request);

                list.Add(view);
            }
            
            return View(list);
        }
    }
}