using System;
using System.Threading.Tasks;

using AutoMapper;

using GameSpace.Infrstructure;
using GameSpace.Models.Tournaments;
using GameSpace.Services.Regions.Contracts;
using GameSpace.Services.Tournaments.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static GameSpace.Common.GlobalConstants.Tournament;

namespace GameSpace.Controllers
{
    public class TournamentController : Controller
    {
        private readonly IRegionService regions;
        private readonly IMapper mapper;
        private readonly ITournamentService tournaments;

        public TournamentController(IRegionService regions, IMapper mapper, ITournamentService tournaments)
        {
            this.regions = regions;
            this.mapper = mapper;
            this.tournaments = tournaments;
        }

        public IActionResult Upcoming()
        {
            var tournamentsService = this.tournaments.AllUpcomingTournaments(onlyVerified: true);



            return View();
        }

        [Authorize]
        public IActionResult Create()
            => View(new CreateTournamentFormModel
            {
                Regions = this.regions.AllRegions(),
                BracketTypes = this.tournaments.AllBracketTypes(),
                MaximumTeamsFormats = this.tournaments.AllMaximumTeamsFormats(),
                TeamSizes = this.tournaments.AllTeamSizes(),
                Maps = this.tournaments.AllMaps(),
                Modes = this.tournaments.AllModes()
            });

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateTournamentFormModel tournament)
        {
            if (!this.regions.RegionExists(tournament.RegionId))
            {
                this.ModelState.AddModelError(nameof(tournament.RegionId), "Region does not exist");
            }

            if (!tournaments.BracketTypeExists(tournament.BracketTypeId))
            {
                this.ModelState.AddModelError(nameof(tournament.BracketTypeId), "Bracket Type does not exist");
            }

            if (!tournaments.MapExists(tournament.MapId))
            {
                this.ModelState.AddModelError(nameof(tournament.MapId), "Map does not exist");
            }

            if (!tournaments.ModeExists(tournament.ModeId))
            {
                this.ModelState.AddModelError(nameof(tournament.ModeId), "Mode does not exist");
            }

            if (!this.tournaments.TeamSizeExists(tournament.TeamSizeId))
            {
                this.ModelState.AddModelError(nameof(tournament.TeamSizeId), "Team Size does not exist");
            }

            var StartOfTournament = tournament.StartsOn.ToUniversalTime();

            if (StartOfTournament.AddMinutes(10).Subtract(DateTime.UtcNow).Days < MinDifferenceDaysInSchedule)
            {
                this.ModelState.AddModelError(nameof(tournament.StartsOn), $"Tournament must be at least {MinDifferenceDaysInSchedule} days in the future.");
            }

            if (StartOfTournament.Subtract(DateTime.UtcNow).Days > MaxDifferenceDaysInSchedule)
            {
                this.ModelState.AddModelError(nameof(tournament.StartsOn), $"Tournament must be at most {MaxDifferenceDaysInSchedule} days in the future.");
            }

            if (!this.ModelState.IsValid)
            {
                var tournamentForm = mapper.Map<CreateTournamentFormModel>(tournament);

                tournamentForm.Regions = regions.AllRegions();
                tournamentForm.BracketTypes = tournaments.AllBracketTypes();
                tournamentForm.MaximumTeamsFormats = tournaments.AllMaximumTeamsFormats();
                tournamentForm.TeamSizes = tournaments.AllTeamSizes();
                tournamentForm.Maps = tournaments.AllMaps();
                tournamentForm.Modes = tournaments.AllModes();
                tournamentForm.StartsOn = StartOfTournament;

                return View(tournamentForm);
            }

            await this.tournaments.AddInPending(
                tournament.Name,
                tournament.Information,
                StartOfTournament,
                tournament.PrizePool,
                tournament.TicketPrize,
                tournament.BronzeMatch,
                tournament.MinimumTeams,
                tournament.CheckInPeriod,
                tournament.GoToGamePeriod,
                tournament.RegionId,
                tournament.BracketTypeId,
                tournament.MapId,
                tournament.MaximumTeamsId,
                tournament.ModeId,
                tournament.TeamSizeId,
                this.User.Id());

            TempData[WebConstants.GlobalMessageKey] = "Your tournament was added and is awaiting for approval!";

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}