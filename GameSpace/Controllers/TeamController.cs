using System.Linq;
using System.IO;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Models.Teams;
using GameSpace.Models.SocialNetworks;
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

        public IActionResult Mine()
        {
            var MyTeam = this.teams.ByUser(this.User.Id());

            return View(MyTeam);
        }

        //public IActionResult Edit(int id)
        //{
        //    var teamData = this.data.Teams.FirstOrDefault(t => t.Id == id);

        //    if (teamData == null)
        //    {
        //        return NotFound();
        //    }

        //    var team = new EditTeamViewModel
        //    {
        //        Id = teamData.Id,
        //        Name = teamData.Name,
        //        Description = teamData.Description,
        //        VideoUrl = teamData.VideoUrl,
        //        WebsiteUrl = teamData.WebsiteUrl,
        //        SocialNetworkId = teamData.SocialNetworksId,
        //        SocialNetwork = new SocialNetworkViewModel
        //        {
        //            FacebookUrl = teamData.SocialNetwork?.FacebookUrl
        //        }
        //    };

        //    return View(team);
        ////}

        //[HttpPost]
        //public async Task<IActionResult> Edit(EditTeamViewModel model)
        //{
        //    var teamData = this.data.Teams.FirstOrDefault(t => t.Id == model.Id);

        //    if (teamData == null)
        //    {
        //        return NotFound();
        //    }

        //    if (teamData.SocialNetworksId != null)
        //    {
        //        this.data.SocialNetworks.Remove(teamData.SocialNetwork);


        //    }

        //    this.data.Teams.Remove(teamData);

        //    teamData = new Team
        //    {
        //        Id = model.Id,
        //        Name = model.Name,
        //        Description = model.Description,
        //        VideoUrl = model.VideoUrl,
        //        WebsiteUrl = model.WebsiteUrl,
        //        //SocialNetwork = new SocialNetwork
        //        //{
        //        //    Id = model.SocialNetworkId ?? default(int),
        //        //    FacebookUrl = model.SocialNetwork?.FacebookUrl
        //        //}
        //    };

        //    //if (teamData.SocialNetworksId != null)
        //    //{
        //    //    teamData.SocialNetwork = new SocialNetwork
        //    //    {
        //    //        Id = model.SocialNetworkId ?? 0,
        //    //        FacebookUrl = model.SocialNetwork?.FacebookUrl
        //    //    };
        //    //}

        //    await this.data.Teams.AddAsync(teamData);
        //    await this.data.SaveChangesAsync();

        //    return RedirectToAction("Index", "Home");
        //}

        [Authorize]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(AddTeamServiceModel team, IFormFile image)
        {
            var nameExcists = teams.NameExcists(team.Name);

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

            var teamId = await teams.Create(team.Name, imageBytes, currUserId);

            await teams.AddMember(teamId, currUserId);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}