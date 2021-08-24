using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using AutoMapper;

using GameSpace.Infrstructure;
using GameSpace.Models.Summoners;
using GameSpace.Services.Regions.Contracts;
using GameSpace.Services.Sumonners.Contracts;
using GameSpace.Services.Sumonners.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GameSpace.Controllers
{
    public class SummonerController : Controller
    {
        private IConfiguration configuration;
        private readonly ISummonerService summoners;
        private readonly IRegionService regions;
        private readonly IMapper mapper;

        public SummonerController(IConfiguration configuration, ISummonerService summoners, IRegionService regions, IMapper mapper)
        {
            this.configuration = configuration;
            this.summoners = summoners;
            this.regions = regions;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult Add()
        {
            var regions = this.mapper.Map<List<SummonerRegionServiceModel>>(this.regions.AllRegions());

            return View(new AddSummonerFormModel
            {
                Regions = regions
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddSummonerFormModel summoner)
        {
            var riotApiKey = this.configuration.GetValue(typeof(string), "RiotApiKey");

            if (!this.regions.RegionExists(summoner.RegionId))
            {
                this.ModelState.AddModelError(nameof(summoner.RegionId), $"Region does not exist.");
            }

            var regionName = this.regions.GetRegionName(summoner.RegionId);

            if (this.summoners.AlreadyAdded(summoner.Name, regionName, this.User.Id()))
            {
                this.ModelState.AddModelError(nameof(summoner.Name), $"Summoner is already added");
            }

            if (this.summoners.AlreadySummonerWithRegion(this.User.Id(), regionName))
            {
                this.ModelState.AddModelError(nameof(summoner.RegionId), $"You can have only one summoner per region.");
            }

            var summonerData = await this.summoners.GetJsonInfoBySummonerName(summoner.Name, regionName);

            if (summonerData.Status?.StatusCode == HttpStatusCode.NotFound)
            {
                this.ModelState.AddModelError(nameof(summoner.Name), summonerData.Status.Message);
            }

            if (!this.ModelState.IsValid)
            {
                summoner.Regions = this.mapper.Map<List<SummonerRegionServiceModel>>(this.regions.AllRegions());

                return View(summoner);
            }

            var profileIcon = await this.summoners.GetProfileImage(summonerData.ProfileIconId);

            await this.summoners.Add(this.User.Id(), summonerData.AccountId ,summonerData.Name, summoner.RegionId, profileIcon);

            return RedirectToAction(nameof(UserController.Profile), "User");
        }

        [Authorize]
        public async Task<IActionResult> Refresh(SummonerQueryModel summonerQueryModel)
        {
            var timer = summonerQueryModel.Timer;
            var userId = summonerQueryModel.UserId;

            if (timer.Hour < 1 && timer.Minute < 1 && timer.Second < 15) //TODO: after 1 hor 15 sex pause again (bug)
            {
                return RedirectToAction(nameof(UserController.Profile), "User", new { userId = userId });
            }

            var accountId = summonerQueryModel.AccountId;

            if (!this.summoners.AccountExists(userId, accountId))
            {
                return RedirectToAction(nameof(UserController.Profile), "User");
            }

            var summonerData = await this.summoners.GetJsonInfoByAccountId(accountId, summonerQueryModel.RegionName);

            var profileIcon = await this.summoners.GetProfileImage(summonerData.ProfileIconId);

            await this.summoners.Refresh(userId, accountId, summonerData.Name, profileIcon);//TODO: Verify logic

            return RedirectToAction(nameof(UserController.Profile), "User", new { userId = userId });
        }

        [Authorize]
        public async Task<IActionResult> Remove(string accountId)
        {
            await this.summoners.Remove(this.User.Id(), accountId);

            return RedirectToAction(nameof(UserController.Profile), "User"/*, new { userId = this.User.Id() }*/);
        }

        [Authorize]
        public async Task<IActionResult> Verify(string accountId, string regionName)
        {
            var summonerData = await this.summoners.GetJsonInfoByAccountId(accountId, regionName);

            var summonerServiceModel = await this.summoners.RandomDefaultIcon(accountId, regionName, summonerData.ProfileIconId);

            return View(summonerServiceModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Verify(VerifySummonerServiceModel verifyModel)
        {
            var sumonerData = await this.summoners.GetJsonInfoByAccountId(verifyModel.AccountId, verifyModel.RegionName);

            if (sumonerData.ProfileIconId == verifyModel.IconId)
            {
                await this.summoners.Verify(sumonerData.AccountId);
            }

            return RedirectToAction(nameof(UserController.Profile), "User");
        }
    }
}