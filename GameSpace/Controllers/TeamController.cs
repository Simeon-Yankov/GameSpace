using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Models.Teams;

namespace GameSpace.Controllers
{
    public class TeamController : Controller
    {
        private readonly GameSpaceDbContext data;

        public TeamController(GameSpaceDbContext data)
            => this.data = data;

        //public IActionResult Edit(int id) => View();

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(AddTeamFormModel team)
        {
            var nameExcists = data.Teams.Any(t => t.Name == team.Name);

            if (nameExcists)
            {
                this.ModelState.AddModelError(nameof(team.Name), $"There is already a team with name '{team.Name}'.");
            }

            if (!ModelState.IsValid)
            {
                return View(team);
            }

            var teamData = new Team
            {
                Name = team.Name,
                CreatedOn = DateTime.UtcNow
            };

            await this.data.Teams.AddAsync(teamData);
            await this.data.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}