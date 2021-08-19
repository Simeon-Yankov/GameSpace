﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using GameSpace.Infrstructure;
using GameSpace.Models.Teams;
using GameSpace.Models.Tournaments;
using GameSpace.Services.Regions.Contracts;
using GameSpace.Services.Teams.Contracts;
using GameSpace.Services.Teams.Models;
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
        public async Task<IActionResult> CheckIn(int tournamentId)
        {
            int teamId = GetRegistratedTeam(tournamentId).Id;

            if (teamId == 0)
            {
                return BadRequest();
            }

            await this.tournaments.CheckInParticipant(tournamentId, teamId, this.User.Id());

            TempData[GlobalMessageKey] = "You Have Successfully Checked In";

            return RedirectToAction(nameof(HomeController.Index), "Home");
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

            tournamentsView.Participants = this.tournaments.TournamentParticipants(tournamentId);

            var isUserAlreadyRegistered = IsUserAlreadyRegistered(tournamentId);

            if (isUserAlreadyRegistered)
            {
                var registeredTeamId = GetRegistratedTeam(tournamentId).Id;

                tournamentsView.IsTeamChecked = this.tournaments.IsTeamChecked(tournamentId, registeredTeamId);

                tournamentsView.IsUserChecked = this.tournaments.IsUserChecked(tournamentId, registeredTeamId, this.User.Id());
            }

            tournamentsView.IsRegistrated = isUserAlreadyRegistered;

            tournamentsView.IsHoster = this.tournaments.IsHoster(this.User.Id(), tournamentsView.HosterName);

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
            }); ;
        }

        [Authorize]
        public IActionResult Selection(int tournamentId, int selectedTeamId)
        {
            var teamMembersService = this.teams.Members(this.User.Id(), selectedTeamId);

            teamMembersService.TournamentId = tournamentId;

            return View(teamMembersService);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Selection(int tournamentId, int selectedTeamId, TeamMembersServiceModel teamMembers)
        {
            var teamMembersService = this.teams.Members(this.User.Id(), selectedTeamId);

            var members = teamMembersService.Members.ToList().OrderBy(m => m.Nickname).ToList();

            var listIsSelected = new List<bool>()
            {
                teamMembers.IsFirstMemberSelected,
                teamMembers.IsSecondMemberSelected,
                teamMembers.IsThirdMemberSelected,
                teamMembers.IsForthMemberSelected,
                teamMembers.IsFifthMemberSelected,
                teamMembers.IsSixthMemberSelected
            };

            var dic = new Dictionary<TeamMemberServiceModel, bool>();

            for (int i = 0; i < members.Count(); i++)
            {
                dic.Add(members[i], listIsSelected[i]);
            }

            var teamSize = this.tournaments.GetTeamSize(tournamentId);

            var owner = members.Where(k => k.IsMemberOwner == true).FirstOrDefault();

            if (dic[owner] == false)
            {
                this.ModelState.AddModelError("All", $"Owner must be involved.");
            }

            if (listIsSelected.Count(x => x == true) != teamSize)
            {
                this.ModelState.AddModelError("All", $"You need exact '{teamSize}' members selected.");
            }

            foreach (var member in members)
            {
                if (dic[member] == true)
                {
                    if (IsUserAlreadyRegistered(tournamentId, member.Id))
                    {
                        var message = member.Id == this.User.Id() ? "You are already registrated." : $"'{member.Nickname}' is already registrated in the Tournament.";

                        this.ModelState.AddModelError("All", message);
                    }
                }
            }

            if (!this.teams.Excists(selectedTeamId))
            {
                this.ModelState.AddModelError("All", "Team does not exists.");
            }
            else if (this.tournaments.IsFull(tournamentId))
            {
                this.ModelState.AddModelError("All", "Tournament is full.");
            }
            else if (!this.tournaments.HasAlreadyStarted(tournamentId))
            {
                this.ModelState.AddModelError("All", "The event has already started.");
            }

            if (!this.ModelState.IsValid)
            {
                teamMembersService.TournamentId = tournamentId;

                teamMembersService.IsFirstMemberSelected = teamMembers.IsFirstMemberSelected;
                teamMembersService.IsSecondMemberSelected = teamMembers.IsSecondMemberSelected;
                teamMembersService.IsThirdMemberSelected = teamMembers.IsThirdMemberSelected;
                teamMembersService.IsForthMemberSelected = teamMembers.IsForthMemberSelected;
                teamMembersService.IsFifthMemberSelected = teamMembers.IsFifthMemberSelected;
                teamMembersService.IsSixthMemberSelected = teamMembers.IsSixthMemberSelected;

                return View(teamMembersService);
            }

            var SelectedMembersId = new List<string>();

            foreach (var member in members)
            {
                if (dic[member] == true)
                {
                    SelectedMembersId.Add(member.Id);
                }
            }

            await this.tournaments.RegisterTeam(tournamentId, selectedTeamId, SelectedMembersId);

            TempData[GlobalMessageKey] = $"Successfully registered Team";

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> Participation(int tournamentId, int selectedTeamId)
        //{
        //    if (IsUserAlreadyRegistrated(tournamentId))
        //    {
        //        this.ModelState.AddModelError("All", "You are already registrated");
        //    }
        //    else if (!this.teams.Excists(selectedTeamId))
        //    {
        //        this.ModelState.AddModelError("All", "Team does not exists.");
        //    }
        //    else if (this.tournaments.IsFull(tournamentId))
        //    {
        //        this.ModelState.AddModelError("All", "Tournament is full");
        //    }
        //    else if (!this.tournaments.HasAlreadyStarted(tournamentId))
        //    {
        //        this.ModelState.AddModelError("All", "The event has already started.");
        //    }

        //    if (!this.ModelState.IsValid)
        //    {
        //        var teamsService = this.teams.ByOwner(this.User.Id());

        //        var teamsView = this.mapper.Map<List<TeamViewModel>>(teamsService);

        //        return View(new ParticipationTournamentViewModel
        //        {
        //            Id = tournamentId,
        //            Teams = teamsView
        //        });
        //    }

        //    await this.tournaments.RegisterTeam(tournamentId, selectedTeamId);

        //    TempData[GlobalMessageKey] = $"Successfully registered Team";

        //    return RedirectToAction(nameof(HomeController.Index), "Home");
        //}

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

        private (IEnumerable<TeamServiceModel>, IEnumerable<TeamServiceModel>) GetParticipantsAndMemberships(int tournamentId, string userId = null)
        {
            if (userId is null)
            {
                userId = this.User.Id();
            }

            var participants = this.tournaments.TournamentParticipants(tournamentId);

            var memberships = this.teams.UserMemberships(userId).ToList();

            return (participants, memberships);
        }

        private TeamServiceModel GetRegistratedTeam(int tournamentId)
        {
            var (participants, memberships) = GetParticipantsAndMemberships(tournamentId);

            var userRegisteredTeams = participants
                                        .Where(p => memberships.Any(m => m.Id == p.Id))
                                        .Select(p => new
                                        {
                                            Id = p.Id,
                                            RegisteredMembers = p.RegistratedMembers
                                        })
                                        .ToList();

            var teamService = memberships.Where(m => participants.FirstOrDefault(p => p.Id == m.Id).Id == m.Id).FirstOrDefault();

            return teamService;
        }

        private bool IsUserAlreadyRegistered(int tournamentId, string userId = null)
        {
            if (userId is null)
            {
                userId = this.User.Id();
            }

            var (participants, memberships) = GetParticipantsAndMemberships(tournamentId, userId);

            var userRegisteredTeams = participants
                                        .Where(p => memberships.Any(m => m.Id == p.Id))
                                        .Select(p => new
                                        {
                                            Id = p.Id,
                                            RegisteredMembers = p.RegistratedMembers
                                        })
                                        .ToList();

            return userRegisteredTeams.Any(x => x.RegisteredMembers.Any(m => m.UserId == userId));
            //return participants.Any(p => memberships.Any(m => m.Id == p.Id));
        }
    }
}