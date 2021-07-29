using System.IO;
using System.Threading.Tasks;

using GameSpace.Services.Teams.Contracts;
using GameSpace.Services.Teams.Models;
using GameSpace.Infrstructure;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace GameSpace.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService teams;

        public TeamController(ITeamService teams)
        {
            this.teams = teams;
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
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(AddTeamServiceModel team, IFormFile image)
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

            await this.teams.Edit( team);

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