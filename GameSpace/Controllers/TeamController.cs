using System.IO;
using System.Threading.Tasks;

using GameSpace.Models.Teams;
using GameSpace.Models.Messages;
using GameSpace.Services.Messages.Contracts;
using GameSpace.Services.Teams.Contracts;
using GameSpace.Services.Teams.Models;
using GameSpace.Services.Users.Contracts;
using GameSpace.Infrstructure;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace GameSpace.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService teams;
        private readonly IUserService users;
        private readonly IMessageService messages;

        public TeamController(ITeamService teams, IUserService users, IMessageService messages)
        {
            this.teams = teams;
            this.users = users;
            this.messages = messages;
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var teamData = teams.Details(id);

            return View(teamData);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var MyTeamData = this.teams.ByUser(this.User.Id());

            return View(MyTeamData);
        }

        [Authorize]
        public IActionResult Invite(int teamId) /*=> View(teamId);*/
        {
            var modelData = new InviteTeamFormModel
            {
                Id = teamId
            };

            return View(modelData);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Invite(InviteTeamFormModel model) // model too generic
        {
            if (model.Username is null)
            {
                this.ModelState.AddModelError(nameof(model.Username), "User is required.");
            }

            var teamId = model.Id;

            if (!this.teams.Excists(teamId))
            {
                return BadRequest(); // invalid operation exception
            }

            if (!this.users.UserExcists(model.Username))
            {
                this.ModelState.AddModelError(nameof(model.Username), "There is no user with given name.");
            }

            if (!this.ModelState.IsValid) //x2
            {
                return View(model);
            }

            var senderId = this.User.Id();

            if (!this.teams.IsMemberInTeam(teamId, senderId))
            {
                return BadRequest();
            }

            var reciverId = this.users.Id(model.Username);

            if (this.teams.IsMemberInTeam(teamId, reciverId))
            {
                this.ModelState.AddModelError(nameof(model.Username), $"The user '{model.Username}' is already a member of the team.");
            }

            if (!this.ModelState.IsValid) //x2
            {
                return View(model);
            }

            var teamName = this.teams.GetName(teamId);

            if (!this.messages.IsRequestSend(reciverId, teamName))
            {
                await this.teams.SendInvitation(senderId, reciverId, teamName);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Authorize]
        public async Task<IActionResult> AcceptInvitation(int id)
        {
            var messageData = this.messages.Get(id);

            var teamId = this.teams.GetId(messageData.TeamName);

            var isMember = this.teams.IsMemberInTeam(teamId, messageData.ReciverId);

            if (!isMember)
            {
                await this.teams.AddMember(teamId, messageData.ReciverId);
            }

            this.messages.Delete(messageData.Id);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Authorize]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(AddServiceModel team, IFormFile image)
        {
            var nameExcists = this.teams.NameExcists(team.Name);

            if (nameExcists)
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
        public IActionResult Edit(int id)
        {
            if (!this.teams.Excists(id))
            {
                return NotFound();
            }

            var teamData = teams.Details(id);

            return View(teamData);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(TeamDetailsServiceModel team)
        {
            if (!this.teams.Excists(team.Id))
            {
                return NotFound(); //TODO: NotFound redirect.
            }

            if (this.teams.ExcistsWantedName(teams.GetName(team.Id), team.Name))
            {
                this.ModelState.AddModelError(nameof(team.Name), "There is already team with this name.");
            }

            if (!this.ModelState.IsValid)
            {
                return View(team);
            }

            await this.teams.Edit(team);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (!this.teams.Excists(id))
            {
                return RedirectToAction(nameof(TeamController.Mine), "Team");
            }

            await teams.Delete(id);

            return RedirectToAction(nameof(TeamController.Mine), "Team");
        }
    }
}