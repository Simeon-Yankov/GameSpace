using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using GameSpace.Infrstructure;
using GameSpace.Models.Teams;
using GameSpace.Models.Tournaments;
using GameSpace.Services.Regions.Contracts;
using GameSpace.Services.Teams.Contracts;
using GameSpace.Services.Tournaments.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static GameSpace.Common.GlobalConstants.Tournament;
using static GameSpace.WebConstants;

namespace GameSpace.Controllers
{
    public class TournamentController : Controller
    {
        private readonly IRegionService regions;
        private readonly IMapper mapper;
        private readonly ITeamService teams;
        private readonly ITournamentService tournaments;

        public TournamentController(
            IRegionService regions,
            IMapper mapper,
            ITeamService teams,
            ITournamentService tournaments)
        {
            this.regions = regions;
            this.mapper = mapper;
            this.teams = teams;
            this.tournaments = tournaments;
        }

        [Authorize]
        public IActionResult Details(int tournamentId)
        {
            var tournament = this.tournaments.Details(tournamentId);

            if (tournament.IsVerified == false)
            {
                return BadRequest();
            }

            if (tournament.StartsOn < DateTime.UtcNow)
            {
                return BadRequest();
            }

            var tournamentsView = this.mapper.Map<TournamentViewModel>(tournament);

            return View(tournamentsView);
        }

        [Authorize]
        public IActionResult Participation(int tournamentId)
        {
            var teamsService = this.teams.ByOwner(this.User.Id());

            var teamsView = this.mapper.Map<List<TeamViewModel>>(teamsService);

            return View(new ParticipationTournamentViewModel
            { 
                Id = tournamentId,
                Teams = teamsView
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Participation(int tournamentId, int selectedTeamId)
        {
            if (!this.teams.Excists(selectedTeamId))
            {
                this.ModelState.AddModelError("All", "Team does not exists.");
            }

            if (this.tournaments.IsTeamAlreadyRegistrated(tournamentId, selectedTeamId))
            {
                this.ModelState.AddModelError("All", "Team is already registered.");
            }

            if (this.tournaments.IsFull(tournamentId))
            {
                this.ModelState.AddModelError("All", "Tournament is already full");
            }

            if (!this.tournaments.HasAlreadyStarted(tournamentId))
            {
                this.ModelState.AddModelError("All", "The event has already started.");
            }

            if (!this.ModelState.IsValid)
            {
                var teamsService = this.teams.ByOwner(this.User.Id());

                var teamsView = this.mapper.Map<List<TeamViewModel>>(teamsService);

                return View(new ParticipationTournamentViewModel
                {
                    Id = tournamentId,
                    Teams = teamsView
                });
            }

            await this.tournaments.RegisterTeam(tournamentId, selectedTeamId);

            TempData[GlobalMessageKey] = $"Successfully registered Team";

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Upcoming()
        {
            var tournamentsService = this.tournaments.AllUpcomingTournaments(onlyVerified: true);

            var tournamentsView = this.mapper.Map<List<TournamentViewModel>>(tournamentsService);

            return View(tournamentsView);
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