using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using GameSpace.Infrstructure;
using GameSpace.Models.Teams;
using GameSpace.Models.Tournaments;
using GameSpace.Services.Algorithms.Contracts;
using GameSpace.Services.Regions.Contracts;
using GameSpace.Services.Sumonners.Contracts;
using GameSpace.Services.Teams.Contracts;
using GameSpace.Services.Teams.Models;
using GameSpace.Services.Tournaments.Contracts;
using GameSpace.Services.Users.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static GameSpace.Common.GlobalConstants.Tournament;
using static GameSpace.WebConstants;

namespace GameSpace.Controllers
{
    public class TournamentController : Controller
    {
        private readonly IAlgorithmService algorithms;
        private readonly IRegionService regions;
        private readonly IMapper mapper;
        private readonly ISummonerService summoners;
        private readonly ITeamService teams;
        private readonly ITournamentService tournaments;
        private readonly IUserService users;

        public TournamentController(
            IAlgorithmService algorithms,
            IRegionService regions,
            IMapper mapper,
            ISummonerService summoners,
            ITeamService teams,
            ITournamentService tournaments,
            IUserService users)
        {
            this.algorithms = algorithms;
            this.regions = regions;
            this.mapper = mapper;
            this.summoners = summoners;
            this.teams = teams;
            this.tournaments = tournaments;
            this.users = users;
        }

        [Authorize]
        public async Task<IActionResult> Administration(int tournamentId)
        {
            var detailsService = await this.tournaments.DetailsAsync(tournamentId);

            var nickname = await this.users.GetNicknameAsync(this.User.Id());

            var isHoster = nickname == detailsService.HosterName;

            if (!isHoster)
            {
                return BadRequest();
            }

            //if (!detailsService.HasBegun) //TODO: VERY IMPORTANT FOR TESTING PUR
            //{
            //    return BadRequest();
            //}

            var checkedInTeams = await this.tournaments.CheckedInTeamsAsync(tournamentId);

            var checkedInTeamsIdNamePair = await this.tournaments.CheckedInTeamsKvpAsync(tournamentId);

            var teamsSeeds = algorithms.SingleEliminationFirstRoundSeeds(checkedInTeamsIdNamePair.OrderBy(t => t.Id).ToList());

            //teamsSeeds.TeamsSeeds[3].IsEliminated = true;
            //teamsSeeds.TeamsSeeds[1].IsEliminated = true;

            //teamsSeeds = this.algorithms.SingleEliminationSecondRound(teamsSeeds);

            //teamsSeeds.TeamsSeeds[0].IsEliminated = true;

            //teamsSeeds = this.algorithms.SingleEliminationThirdRound(teamsSeeds);

            return View(new AdministrationTournamentViewModel
            {
                IsHoster = isHoster,
                Details = detailsService,
                CheckedInTeams = checkedInTeams,
                CheckedInTeamsIdNamePair = checkedInTeamsIdNamePair.OrderBy(t => t.Id).ToList(),
                TeamSeeds = teamsSeeds
            });
        }

        [Authorize]
        public async Task<IActionResult> CheckIn(int tournamentId, int regionId)
        {
            var team = await GetRegistratedTeamAsync(tournamentId);

            int teamId = default;

            if (team is not null)
            {
                teamId = team.Id;
            }

            if (teamId == 0)
            {
                return BadRequest();
            }

            var userId = this.User.Id();

            var regionName = await this.regions.GetRegionNameAsync(regionId);

            if (!await this.summoners.AccountExistsByRegionIdAsync(userId, regionId))
            {
                TempData[GlobalMessageKeyDanger] = $"You must have summoner in {regionName} first to check in.";

                return RedirectToAction(nameof(TournamentController.Details), "Tournament", new { tournamentId = tournamentId });
            }

            if (!await this.summoners.IsVerifiedByRegionAsync(userId, regionId))
            {
                TempData[GlobalMessageKeyDanger] = $"You must verify your summoner first.";

                var accountId = await this.summoners.GetIdByRegionAsync(userId, regionId);

                return RedirectToAction(nameof(SummonerController.Verify), "Summoner", new { accountId = accountId, regionName = regionName });
            }

            await this.tournaments.CheckInParticipant(tournamentId, teamId, userId);

            TempData[GlobalMessageKey] = "You Have Successfully Checked In";

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Authorize]
        public async Task<IActionResult> Details(int tournamentId)
        {
            var tournament = await this.tournaments.DetailsAsync(tournamentId);

            if (tournament.IsVerified == false)
            {
                return BadRequest();
            }

            if (tournament.StartsOn < DateTime.UtcNow)
            {
                return BadRequest();
            }

            var tournamentsView = this.mapper.Map<TournamentViewModel>(tournament);

            tournamentsView.Participants = await this.tournaments.TournamentParticipantsAsync(tournamentId);

            var isUserAlreadyRegistered = await IsUserAlreadyRegisteredAsync(tournamentId);

            if (isUserAlreadyRegistered)
            {
                var registeredTeam = await GetRegistratedTeamAsync(tournamentId);

                tournamentsView.IsTeamChecked = await this.tournaments.IsTeamCheckedAsync(tournamentId, registeredTeam.Id);

                tournamentsView.IsUserChecked = await this.tournaments.IsUserCheckedAsync(
                    tournamentId,
                    registeredTeam.Id,
                    this.User.Id());
            }

            tournamentsView.IsRegistrated = isUserAlreadyRegistered;

            tournamentsView.IsHoster = await this.tournaments.IsHosterAsync(this.User.Id(), tournamentsView.HosterName);

            return View(tournamentsView);
        }

        [Authorize]
        public async Task<IActionResult> Participation(int tournamentId)
        {
            var teamsService = await this.teams.ByOwner(this.User.Id());

            var teamsView = this.mapper.Map<List<TeamViewModel>>(teamsService);

            return View(new ParticipationTournamentViewModel
            {
                Id = tournamentId,
                Teams = teamsView
            }); ;
        }

        [Authorize]
        public async Task<IActionResult> Selection(int tournamentId, int selectedTeamId)
        {
            var teamMembersService = await this.teams.Members(this.User.Id(), selectedTeamId);

            teamMembersService.TournamentId = tournamentId;

            return View(teamMembersService);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Selection(int tournamentId, int selectedTeamId, TeamMembersServiceModel teamMembers)
        {
            var teamMembersService = await this.teams.Members(this.User.Id(), selectedTeamId);

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

            var teamSize = await this.tournaments.GetTeamSizeAsync(tournamentId);

            var owner = members.Where(k => k.IsMemberOwner == true).FirstOrDefault();

            if (dic[owner] == false)
            {
                this.ModelState.AddModelError("All", $"Owner must be involved.");
            }

            if (listIsSelected.Count(x => x == true) != teamSize)
            {
                this.ModelState.AddModelError("All", $"You need exact {teamSize} members selected.");
            }

            foreach (var member in members)
            {
                if (dic[member] == true)
                {
                    if (await IsUserAlreadyRegisteredAsync(tournamentId, member.Id))
                    {
                        var message = member.Id == this.User.Id() ? "You are already registrated." : $"'{member.Nickname}' is already registrated in the Tournament.";

                        this.ModelState.AddModelError("All", message);
                    }
                }
            }

            if (!await this.teams.Excists(selectedTeamId))
            {
                this.ModelState.AddModelError("All", "Team does not exists.");
            }
            else if (await this.tournaments.IsFullAsync(tournamentId))
            {
                this.ModelState.AddModelError("All", "Tournament is full.");
            }
            else if (!await this.tournaments.HasAlreadyStartedAsync(tournamentId))
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

        public async Task<IActionResult> Upcoming([FromQuery] AllTournamentsQueryModel query)
        {
            var tournamentsService = await this.tournaments.AllUpcomingTournamentsAsync(
                onlyVerified: true,
                searchTerm: query.SearchTerm,
                currentPage: query.CurrentPage,
                carsPerPage: AllTournamentsQueryModel.TournamentsPerPage,
                sorting: query.Sorting);

            var tournamentsView = this.mapper.Map<AllTournamentsQueryModel>(tournamentsService);

            return View(tournamentsView);
        }

        //public IActionResult Upcoming()
        //{
        //    var tournamentsService = this.tournaments.AllUpcomingTournaments(onlyVerified: true);

        //    var tournamentsView = this.mapper.Map<List<TournamentViewModel>>(tournamentsService);

        //    return View(tournamentsView);
        //}

        [Authorize]
        public async Task<IActionResult> Create()
            => View(new CreateTournamentFormModel
            {
                Regions = await this.regions.AllRegionsAsync(),
                BracketTypes = await this.tournaments.AllBracketTypesAsync(),
                MaximumTeamsFormats = await this.tournaments.AllMaximumTeamsFormatsAsync(),
                TeamSizes = await this.tournaments.AllTeamSizesAsync(),
                Maps = await this.tournaments.AllMapsAsync(),
                Modes = await this.tournaments.AllModesAsync()
            });

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateTournamentFormModel tournament)
        {
            if (!await this.regions.RegionExistsAsync(tournament.RegionId))
            {
                this.ModelState.AddModelError(nameof(tournament.RegionId), "Region does not exist");
            }

            if (!await tournaments.BracketTypeExistsAsync(tournament.BracketTypeId))
            {
                this.ModelState.AddModelError(nameof(tournament.BracketTypeId), "Bracket Type does not exist");
            }

            if (!await tournaments.MapExistsAsync(tournament.MapId))
            {
                this.ModelState.AddModelError(nameof(tournament.MapId), "Map does not exist");
            }

            if (!await tournaments.ModeExistsAsync(tournament.ModeId))
            {
                this.ModelState.AddModelError(nameof(tournament.ModeId), "Mode does not exist");
            }

            if (!await this.tournaments.TeamSizeExistsAsync(tournament.TeamSizeId))
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
                var tournamentForm = mapper.Map<CreateTournamentFormModel>(tournament); //TODO: Make Custom map logic

                tournamentForm.Regions = await regions.AllRegionsAsync();
                tournamentForm.BracketTypes = await tournaments.AllBracketTypesAsync();
                tournamentForm.MaximumTeamsFormats = await tournaments.AllMaximumTeamsFormatsAsync();
                tournamentForm.TeamSizes = await tournaments.AllTeamSizesAsync();
                tournamentForm.Maps = await tournaments.AllMapsAsync();
                tournamentForm.Modes = await tournaments.AllModesAsync();
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

            TempData[WebConstants.GlobalMessageKey] = "Your tournament was added and is waiting for approval!";

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private async Task<(IEnumerable<TeamServiceModel>, IEnumerable<TeamServiceModel>)> GetParticipantsAndMemberships(int tournamentId, string userId = null)
        {
            if (userId is null)
            {
                userId = this.User.Id();
            }

            var participants = await this.tournaments.TournamentParticipantsAsync(tournamentId);

            var memberships = await this.teams.UserMemberships(userId);

            return (participants, memberships);
        }

        private async Task<TeamServiceModel> GetRegistratedTeamAsync(int tournamentId)
        {
            var (participants, memberships) = await GetParticipantsAndMemberships(tournamentId);

            var userRegisteredTeams = participants
                                        .Where(p => memberships.Any(m => m.Id == p.Id))
                                        //.Select(p => new 
                                        //{
                                        //    Id = p.Id,
                                        //    RegisteredMembers = p.RegistratedMembers
                                        //})
                                        .ToList();

            //var teamService = memberships.Where(m => participants.FirstOrDefault(p => p.Id == m.Id).Id == m.Id).FirstOrDefault();

            var teamService = userRegisteredTeams
                                .Where(t => t.RegistratedMembers
                                .Any(m => m.UserId == this.User.Id()))
                                .FirstOrDefault();

            return teamService;
        }

        private async Task<bool> IsUserAlreadyRegisteredAsync(int tournamentId, string userId = null)
        {
            if (userId is null)
            {
                userId = this.User.Id();
            }

            var (participants, memberships) = await GetParticipantsAndMemberships(tournamentId, userId);

            var userRegisteredTeams = participants
                                        .Where(p => memberships.Any(m => m.Id == p.Id))
                                        .Select(p => new
                                        {
                                            Id = p.Id,
                                            RegisteredMembers = p.RegistratedMembers
                                        })
                                        .ToList();

            return userRegisteredTeams.Any(x => x.RegisteredMembers.Any(m => m.UserId == userId));
        }
    }
}