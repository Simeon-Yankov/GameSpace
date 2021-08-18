using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.Tournaments.Contracts;
using GameSpace.Services.Tournaments.Models;

using static GameSpace.Common.GlobalConstants;
using static GameSpace.Common.GlobalConstants.Tournament;

namespace GameSpace.Services.Tournaments
{
    public class TournamentService : ITournamentService
    {
        private readonly GameSpaceDbContext data;

        public TournamentService(GameSpaceDbContext data)
            => this.data = data;


        public TournamentServiceModel Details(int tournamentId)
        {
           var tournament = GetQueryableTournament(tournamentId).FirstOrDefault();

            return new TournamentServiceModel
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Information = tournament.Information,
                StartsOn = tournament.StartsOn,
                PrizePool = tournament.PrizePool,
                TicketPrize = tournament.TicketPrize,
                BronzeMatch = tournament.BronzeMatch,
                MinimumTeams = tournament.MinimumTeams,
                CheckInPeriod = tournament.CheckInPeriod,
                GoToGamePeriod = tournament.GoToGamePeriod,
                RegionId = tournament.RegionId,
                BracketTypeId = tournament.BracketTypeId,
                MapId = tournament.MapId,
                MaximumTeamsId = tournament.MaximumTeamsId,
                ModeId = tournament.ModeId,
                TeamSizeId = tournament.TeamSizeId,
                IsVerified = tournament.IsVerified,
                HosterId = tournament.HosterId,
                HosterName = GetHosterName(tournament.HosterId), //TODO: CASE FOR VALIDATION
                RegionName = GetRegionName(tournament.RegionId),
                MaximumTeams = GetCapacity(tournament.MaximumTeamsId),
                TeamSizeFormat = GetFormat(tournament.TeamSizeId),
                BracketTypeFormat = GetBracketType(tournament.BracketTypeId),
                StartsInMessage = GetStartsInMessage(tournament.StartsOn)
            };
        }
                 
        //TODO: INTRODUCE AUTOMAP IN SERVICE LAYER
        public IEnumerable<TournamentServiceModel> AllUpcomingTournaments(
            int daysFrom = default,
            int daysTo = MaxDifferenceDaysInSchedule,
            string orderBy = "date",
            bool onlyVerified = false)
        {
            var utcNow = DateTime.UtcNow;

            var tournamentsData = this.data
                .TeamsTournaments
                .Where(tt => !tt.IsFinished)
                .ToList();

            if (onlyVerified)
            {
                tournamentsData = tournamentsData
                    .Where(t => t.IsVerified == onlyVerified)
                    .ToList();
            }

            var tournamentsService = tournamentsData
                .Where(tt => IsValid(tt, daysFrom, daysTo, utcNow))
                .Select(tt => new TournamentServiceModel
                {
                    Id = tt.Id,
                    Name = tt.Name,
                    Information = tt.Information,
                    StartsOn = tt.StartsOn,
                    PrizePool = tt.PrizePool,
                    TicketPrize = tt.TicketPrize,
                    BronzeMatch = tt.BronzeMatch,
                    MinimumTeams = tt.MinimumTeams,
                    CheckInPeriod = tt.CheckInPeriod,
                    GoToGamePeriod = tt.GoToGamePeriod,
                    RegionId = tt.RegionId,
                    BracketTypeId = tt.BracketTypeId,
                    MapId = tt.MapId,
                    MaximumTeamsId = tt.MaximumTeamsId,
                    ModeId = tt.ModeId,
                    TeamSizeId = tt.TeamSizeId,
                    IsVerified = tt.IsVerified,
                    HosterId = tt.HosterId,
                    HosterName = GetHosterName(tt.HosterId), //TODO: CASE FOR VALIDATION
                    RegionName = GetRegionName(tt.RegionId),
                    MaximumTeams = GetCapacity(tt.MaximumTeamsId),
                    TeamSizeFormat = GetFormat(tt.TeamSizeId),
                    BracketTypeFormat = GetBracketType(tt.BracketTypeId),
                    StartsInMessage = GetStartsInMessage(tt.StartsOn)
                })
                .AsEnumerable();

            return OrderTournament(tournamentsService, orderBy);
        }

        public string GetStartsInMessage(DateTime startsOn)
        {
            var diff = startsOn.Subtract(DateTime.UtcNow);

            if (diff.Days > Unit)
            {
                return $"Starts in {diff.Days} days";
            }
            else if (diff.Days == Unit)
            {
                var ending = ReturnEndingIfPlural(diff.Hours);

                return $"Starts in a day and {diff.Hours} hour{ending}";
            }
            else if (diff.Hours > Unit)
            {
                return $"Starts in {diff.Hours} hours";
            }
            else if (diff.Hours == Unit)
            {
                var ending = ReturnEndingIfPlural(diff.Minutes);

                return $"Starts in a hour and {diff.Minutes} minute{ending}";
            }
            else if (diff.Minutes > Unit)
            {
                return $"Starts in {diff.Minutes} minutes";
            }
            else
            {
                return $"Starts in a minute";
            }
        }

        public string GetBracketType(int bracketType)
            => this.data
                .BracketTypes
                .Where(bt => bt.Id == bracketType)
                .Select(bt => bt.Name)
                .FirstOrDefault();

        public int GetCapacity(int capacityId)
            => this.data
                .MaximumTeamsFormats
                .Where(mtf => mtf.Id == capacityId)
                .Select(mtf => mtf.Capacity)
                .FirstOrDefault();

        public string GetFormat(int formatId)
            => this.data
                .TeamSizes
                .Where(f => f.Id == formatId)
                .Select(f => f.Format)
                .FirstOrDefault();

        public string GetRegionName(int regionId) 
            => this.data
                .Regions
                .Where(r => r.Id == regionId)
                .Select(r => r.Name)
                .FirstOrDefault();

        public string GetHosterName(int hosterId)
            => this.data
                .Users
                .Where(u => u.HostTournaments.id == hosterId)
                .Select(u => u.Nickname)
                .FirstOrDefault();

        public int GetHosterId(string userId)
            => this.data
                .Users
                .Where(u => u.Id == userId)
                .Select(u => u.HostTournaments.id)
                .FirstOrDefault();

        public string GetInformation(int tournamentId)
            => GetQueryableTournament(tournamentId)
                .Select(tt => tt.Information)
                .FirstOrDefault();

        public async Task AddInPending(
            string name,
            string information,
            DateTime startsOn,
            decimal prizePool,
            decimal ticketPrize,
            bool bronzeMatch,
            int minimumTeams,
            int checkInPeriod,
            int goToGamePeriod,
            int regionId,
            int bracketId,
            int mapId,
            int maximumTeamsFormatId,
            int modeId,
            int teamSizeId,
            string userId)
        {

            var hosterId = this.GetHosterId(userId);

            var tournament = new TeamsTournament
            {
                Name = name,
                Information = information,
                StartsOn = startsOn,
                PrizePool = prizePool,
                TicketPrize = ticketPrize,
                BronzeMatch = bronzeMatch,
                MinimumTeams = minimumTeams,
                CheckInPeriod = checkInPeriod,
                GoToGamePeriod = goToGamePeriod,
                RegionId = regionId,
                BracketTypeId = bracketId,
                MapId = mapId,
                MaximumTeamsId = maximumTeamsFormatId,
                ModeId = modeId,
                TeamSizeId = teamSizeId,
                HosterId = hosterId,
                IsPending = true
            };

            await AddTournamentToHoster(userId, tournament);
        }

        public async Task RegisterTeam(int tournamentId, int teamId)
        {
            var tournament = GetQueryableTournament(tournamentId).FirstOrDefault();

            var relation = new TeamsTournamentTeam
            {
                TeamId = teamId,
                TeamsTournamentId = tournamentId
            };

            tournament.RegisteredTeams.Add(relation);

            await this.data.SaveChangesAsync();
        }

        public async Task Verify(int tournamentId)
        {
            var tournamentData = GetQueryableTournament(tournamentId).FirstOrDefault();

            tournamentData.IsVerified = true;

            await this.data.SaveChangesAsync();
        }

        public async Task Unverify(int tournamentId)
        {
            var tournamentData = GetQueryableTournament(tournamentId).FirstOrDefault();

            tournamentData.IsVerified = false;

            await this.data.SaveChangesAsync();
        }

        public bool BracketTypeExists(int bracketTypeId)
            => this.data
            .BracketTypes
            .Any(bt => bt.Id == bracketTypeId);

        public bool HasAlreadyStarted(int tournamentId)
            => GetQueryableTournament(tournamentId)
                .Select(tt => tt.StartsOn > DateTime.UtcNow)
                .FirstOrDefault();

        public bool IsFull(int tournamentId)
        {
            return GetQueryableTournament(tournamentId)
                    .Select(tt => tt.RegisteredTeams.Count > tt.MaximumTeamsFormat.Capacity)
                    .FirstOrDefault();
        }

        public bool IsTeamAlreadyRegistrated(int tournamentId, int teamId)
            => GetQueryableTournament(tournamentId)
                .Select(tt => tt.RegisteredTeams
                                .Any(rt => rt.TeamId == teamId))
                .FirstOrDefault();

        public bool MapExists(int mapId) => this.data.Maps.Any(m => m.Id == mapId);

        public bool MaximumTeamsExists(int MaximumTeamsId)
            => this.data
            .MaximumTeamsFormats
            .Any(mt => mt.Id == MaximumTeamsId);

        public bool ModeExists(int modeId) => this.data.Modes.Any(m => m.Id == modeId);

        public bool TeamSizeExists(int teamSizeId)
            => this.data
            .TeamSizes
            .Any(ts => ts.Id == teamSizeId);

        public async Task AddTournamentToHoster(string userId, TeamsTournament teamsTournament)
        {
            var hoster = this.data
               .Users
               .Where(u => u.Id == userId)
               .Select(u => u.HostTournaments)
               .FirstOrDefault();

            hoster.TeamsTournaments.Add(teamsTournament);

            await this.data.SaveChangesAsync();
        }

        public IEnumerable<BracketTypeServiceModel> AllBracketTypes()
            => this.data
                .BracketTypes
                .Select(bt => new BracketTypeServiceModel
                {
                    Id = bt.Id,
                    Name = bt.Name
                })
                .AsEnumerable();

        public IEnumerable<MapServiceModel> AllMaps()
            => this.data
                .Maps
                .Select(ts => new MapServiceModel
                {
                    Id = ts.Id,
                    Name = ts.Name
                })
                .AsEnumerable();

        public IEnumerable<MaximumTeamsFormatServiceModle> AllMaximumTeamsFormats()
            => this.data
                .MaximumTeamsFormats
                .Select(mtf => new MaximumTeamsFormatServiceModle
                {
                    Id = mtf.Id,
                    Capacity = mtf.Capacity
                })
                .AsEnumerable();

        public IEnumerable<ModeServiceModel> AllModes()
            => this.data
                .Modes
                .Select(ts => new ModeServiceModel
                {
                    Id = ts.Id,
                    Name = ts.Name
                })
                .AsEnumerable();

        public IEnumerable<TeamSizeServiceModel> AllTeamSizes()
            => this.data
                .TeamSizes
                .Select(ts => new TeamSizeServiceModel
                {
                    Id = ts.Id,
                    Format = ts.Format
                })
                .AsEnumerable();

        private bool IsValid(TeamsTournament tournament, int daysFrom, int daysTo, DateTime utcNow)
        {
            var isZero = false;
            var isValid = false;

            var diff = tournament.StartsOn.Subtract(utcNow);

            if (diff.Days == daysFrom)
            {
                isValid = diff.Hours <= 0 ?
                            diff.Minutes <= 0 ? false : true
                            : true;
                isZero = true;
            }

            if (diff.Days > daysFrom || isZero && isValid)
            {
                if (diff.Days < daysTo)
                {
                    return true;
                }
            }

            return false;
        }

        private IQueryable<TeamsTournament> GetQueryableTournament(int tournamentId)
            => this.data
                    .TeamsTournaments
                    .Where(tt => tt.Id == tournamentId)
                    .AsQueryable();

        private IEnumerable<TournamentServiceModel> OrderTournament(IEnumerable<TournamentServiceModel> tournament, string orderBy)
        {
            if (orderBy == "date")
            {
                return tournament.OrderBy(t => t.StartsOn);
            }
            else if (orderBy == "verified")
            {
                return tournament.OrderBy(t => t.IsVerified);
            }
            else if (orderBy == "hoster")
            {
                return tournament.OrderBy(t => t.HosterId);
            }
            else if (orderBy == "name")
            {
                return tournament.OrderBy(t => t.HosterId);
            }
            else
            {
                return tournament.OrderBy(t => t.StartsOn);
            }
        }

        private string ReturnEndingIfPlural(int quantity) => quantity == 1 ? string.Empty : "s";
    }
}