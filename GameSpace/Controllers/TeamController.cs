using System.IO;
using System.Threading.Tasks;

using AutoMapper;

using GameSpace.Infrstructure;
using GameSpace.Models.Teams;
using GameSpace.Services.Messages.Contracts;
using GameSpace.Services.Messages.Modles;
using GameSpace.Services.Teams.Contracts;
using GameSpace.Services.Teams.Models;
using GameSpace.Services.Users.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameSpace.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService teams;
        private readonly IUserService users;
        private readonly IMessageService messages;
        private readonly IMapper mapper;

        public TeamController(
            ITeamService teams,
            IUserService users,
            IMessageService messages,
            IMapper mapper)
        {
            this.teams = teams;
            this.users = users;
            this.messages = messages;
            this.mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> Details(int teamId, string userId = null)
        {
            userId ??= this.User.Id();

            var teamData = await teams.Details(teamId, userId);

            return View(teamData);
        }

        [Authorize]
        public IActionResult Invite(int teamId)
        {
            var modelData = new InviteTeamFormModel
            {
                TeamId = teamId
            };

            return View(modelData);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Invite(InviteTeamFormModel model) // model too generic
        {
            if (model.Nickname is null)
            {
                this.ModelState.AddModelError(nameof(model.Nickname), "User is required.");
            }

            var teamId = model.TeamId;

            if (!await this.teams.Excists(teamId))
            {
                return BadRequest(); // invalid operation exception
            }

            if (!this.users.ExcistsByNickname(model.Nickname))
            {
                this.ModelState.AddModelError(nameof(model.Nickname), "There is no existing user with the given name.");
            }

            if (!this.ModelState.IsValid) //x2
            {
                return View(model);
            }

            var senderId = this.User.Id();

            if (!await this.teams.IsMemberInTeam(teamId, senderId))
            {
                return BadRequest();
            }

            if (await this.teams.IsTeamFull(teamId))
            {
                this.ModelState.AddModelError(nameof(model.Nickname), $"Team is already full."); //TODO: maybe do it in summary or redirect to other page
            }

            var reciverId = this.users.Id(model.Nickname);

            if (await this.teams.IsMemberInTeam(teamId, reciverId))
            {
                this.ModelState.AddModelError(nameof(model.Nickname), $"The user '{model.Nickname}' is already a member of the team.");
            }

            if (!this.ModelState.IsValid) //x2
            {
                return View(model);
            }

            var teamName = await this.teams.GetName(teamId);

            if (!this.messages.IsRequestSend(reciverId, teamName))
            {
                await this.teams.SendInvitation(senderId, reciverId, teamName);
            }

            return RedirectToAction(nameof(TeamController.Memberships), "Team");
        }

        [Authorize]
        public async Task<IActionResult> AcceptInvitation(int requestId)
        {
            var messageData = this.messages.Get(requestId);

            if (messageData is null)
            {
                return RedirectToAction(nameof(MessageController.All), "Message");
            }

            var teamName = messageData.TeamName;

            if (!await this.teams.Excists(teamName))
            {
                return RedirectToAction(nameof(MessageController.All), "Message");
            }

            var teamId = await this.teams.GetId(teamName);

            if (await this.teams.IsTeamFull(teamId))
            {
                return RedirectToAction(nameof(MessageController.All), "Message"); // SHOW SOME ERROR
            }

            var isMember = await this.teams.IsMemberInTeam(teamId, messageData.ReciverId);

            if (!isMember)
            {
                await this.teams.AddMember(teamId, messageData.ReciverId);
            }

            await this.messages.Delete(messageData.RequestId);

            await SendNotification(messageData, teamName);

            return RedirectToAction(nameof(MessageController.All), "Message");
        }

        [Authorize]
        public async Task<IActionResult> DeclineInvitation(int requestId)
        {
            await this.messages.Delete(requestId);

            return RedirectToAction(nameof(MessageController.All), "Message");
        }

        [Authorize]
        public async Task<IActionResult> Memberships()
        {
            var MyTeamsData = await this.teams.UserMemberships(this.User.Id());

            return View(MyTeamsData);
        }

        [Authorize]
        public async Task<IActionResult> Members(int teamId)
        {
            var teamMembersData = await this.teams.Members(this.User.Id(), teamId);

            return View(teamMembersData);
        }

        [Authorize]
        public async Task<IActionResult> Leave(int teamId, string memberId = null)
        {

            if (await this.teams.Excists(teamId))
            {
                memberId ??= this.User.Id();
                await this.teams.RemoveMember(teamId, memberId);
            }

            return RedirectToAction(nameof(TeamController.Memberships), "Team");
        }

        [Authorize]
        public async Task<IActionResult> PromoteToOwner(int teamId, string memberId)
        {
            if (!this.users.ExcistsById(memberId)) //TODO: when you del you acc
            {
                return RedirectToAction(nameof(TeamController.Memberships), "Team"); //TODO: maybe throw bad request 3x
            }

            if (!await this.teams.Excists(teamId))
            {
                return RedirectToAction(nameof(TeamController.Memberships), "Team");
            }

            if (!await this.teams.IsMemberInTeam(teamId, memberId))
            {
                return RedirectToAction(nameof(TeamController.Memberships), "Team");
            }

            await this.teams.PromoteToOwner(teamId, memberId);

            return RedirectToAction(nameof(TeamController.Memberships), "Team");
        }

        [Authorize]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(AddServiceModel team, IFormFile image)
        {
            var nameExcists = this.teams.Excists(team.Name);

            if (await nameExcists)
            {
                this.ModelState.AddModelError(nameof(team.Name), $"There is already a team with name '{team.Name}'.");
            }

            if (!ModelState.IsValid)
            {
                return View(team);
            }

            var imageInMemory = new MemoryStream();
            byte[] imageBytes = null;

            if (image is not null)
            {
                await image?.CopyToAsync(imageInMemory);

                imageBytes = imageInMemory.ToArray();
            }

            var currUserId = this.User.Id();

            var teamId = await this.teams.Create(team.Name, imageBytes, currUserId);

            await this.teams.AddMember(teamId, currUserId);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await this.teams.Excists(id))
            {
                return NotFound();
            }

            var teamData = await teams.Details(id, null);

            var teamForm = this.mapper.Map<EditTeamFromModel>(teamData);

            return View(teamForm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditTeamFromModel team)
        {
            if (!await this.teams.Excists(team.Id))
            {
                return BadRequest(); //TODO: NotFound redirect.
            }

            if (await this.teams.ExcistsWantedName(await this.teams.GetName(team.Id), team.Name))
            {
                this.ModelState.AddModelError(nameof(team.Name), "There is already a team with this given name.");
            }

            if (!this.ModelState.IsValid)
            {
                return View(team);
            }

            await this.teams.Edit(
                team.Id,
                team.Name,
                team.Description,
                team.VideoUrl,
                team.WebsiteUrl);

            return RedirectToAction(nameof(TeamController.Details), "Team", new { teamId = team.Id });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int teamId)
        {
            if (!await this.teams.Excists(teamId))
            {
                return RedirectToAction(nameof(TeamController.Memberships), "Team");
            }

            await this.teams.Delete(teamId);

            return RedirectToAction(nameof(TeamController.Memberships), "Team");
        }

        private async Task SendNotification(TeamInvitationMessageServiceModel messageData, string teamName)
        {
            var ownerId = await this.teams.GetOwnerId(teamName);
            var message = $"{messageData.ReciverUsername} has accepted invitation for Team {teamName}.";

            if (messageData.SenderId != ownerId)
            {
                await this.messages.SendNotification(ownerId, message);
            }
            await this.messages.SendNotification(messageData.SenderId, message);
        }
    }
}