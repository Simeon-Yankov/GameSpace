using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Services.Teams.Models;
using GameSpace.Services.Tournaments.Contracts;
using GameSpace.Services.Tournaments.Models;
using GameSpace.Services.Tournaments.Models.Enum;
using Microsoft.EntityFrameworkCore;

using static GameSpace.Common.GlobalConstants;
using static GameSpace.Common.GlobalConstants.Tournament;

namespace GameSpace.Services.Tournaments
{
    public class TournamentService : ITournamentService
    {
        private readonly GameSpaceDbContext data;

        public TournamentService(GameSpaceDbContext data)
            => this.data = data;

        public async Task CheckInParticipant(int tournamentId, int teamId, string userId)
        {
            var tournament = await this.data
                .TeamsTournaments
                .Where(tt => tt.Id == tournamentId)
                .Select(t => t.RegisteredTeams
                    .Where(rt => rt.TeamId == teamId && rt.TeamsTournamentId == tournamentId)
                    .FirstOrDefault())
                .FirstOrDefaultAsync();

            var relationTournamentTeamId = await this.data
                .TeamsTournamentsTeams
                .Where(ttt => ttt.TeamsTournamentId == tournamentId && ttt.TeamId == teamId)
                .Select(ttt => ttt.Id)
                .FirstOrDefaultAsync();

            var member = await this.data
                .UsersTeamsTournamentTeams
                .Where(uttt => uttt.TeamsTournamentTeamId == relationTournamentTeamId && uttt.UserId == userId)
                .FirstOrDefaultAsync();

            member.IsChecked = true;

            await this.data.SaveChangesAsync();

            var AreMembersCheck = await this.data
                .UsersTeamsTournamentTeams
                .Where(uttt => uttt.TeamsTournamentTeamId == relationTournamentTeamId)
                .Select(uttt => new
                {
                    IsChecked = uttt.IsChecked
                })
                .ToListAsync();

            if (AreMembersCheck.All(t => t.IsChecked))
            {
                tournament.IsChecked = true;
            }

            await this.data.SaveChangesAsync();
        }

        public async Task<TournamentServiceModel> DetailsAsync(int tournamentId)
        {
            var details = await this.data
                    .TeamsTournaments
                    .Where(tt => tt.Id == tournamentId)
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
                        HosterName = tt.Hoster.User.Nickname,
                        RegionName = tt.Region.Name,
                        MaximumTeams = tt.MaximumTeamsFormat.Capacity,
                        TeamSizeFormat = tt.TeamSize.Format,
                        BracketTypeFormat = tt.BracketType.Name,
                        MapName = tt.Map.Name,
                        ModeName = tt.Mode.Name,
                    })
                    .FirstOrDefaultAsync();

            details.StartsInMessage = GetStartsInMessage(details.StartsOn);

            return details;
        }

        //TODO: INTRODUCE AUTOMAP IN SERVICE LAYER
        public async Task<AllTournamentsServiceModel> AllUpcomingTournamentsAsync(
            int daysFrom = default,
            int daysTo = MaxDifferenceDaysInSchedule,
            string orderBy = "date",
            bool onlyVerified = false,
            int currentPage = 1,
            int tournamentsPerPage = int.MaxValue,
            string searchTerm = null,
            TournamentSorting sorting = 0)
        {
            var utcNow = DateTime.UtcNow;

            var tournamentsData = this.data
                .TeamsTournaments
                .Where(tt => !tt.IsFinished)
                .AsQueryable();

            if (onlyVerified)
            {
                tournamentsData = tournamentsData
                    .Where(t => t.IsVerified == onlyVerified);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                tournamentsData = tournamentsData.Where(t =>
                    t.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            var tournamentsDataMateriallized = await tournamentsData
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
                    HosterName = tt.Hoster.User.Nickname,
                    RegionName = tt.Region.Name,
                    MaximumTeams = tt.MaximumTeamsFormat.Capacity,
                    TeamSizeFormat = tt.TeamSize.Format,
                    BracketTypeFormat = tt.BracketType.Name,
                    MapName = tt.Map.Name,
                    ModeName = tt.Mode.Name,
                })
                .ToListAsync();

            var tournamentsService = tournamentsDataMateriallized
                .Where(tt => IsValid(tt, daysFrom, daysTo, utcNow))
                .AsEnumerable();

            var OrderedTournaments = OrderTournament(tournamentsService, orderBy, sorting.ToString());

            var OrderedTournamentsGroped = OrderedTournaments
                .Skip((currentPage - 1) * tournamentsPerPage)
                .Take(tournamentsPerPage)
                .AsEnumerable();

            foreach (var tournamentDataMateriallized in tournamentsDataMateriallized)
            {
                tournamentDataMateriallized.StartsInMessage = GetStartsInMessage(tournamentDataMateriallized.StartsOn);
            }

            var totalTournaments = OrderedTournaments.Count();

            var AllTournamentsQueryModle = new AllTournamentsServiceModel
            {
                CurrentPage = currentPage,
                SearchTerm = searchTerm,
                TotalTournaments = totalTournaments,
                Tournaments = OrderedTournamentsGroped
            };

            return AllTournamentsQueryModle;
        }

        public async Task<IEnumerable<IdNamePairTeamServiceModel>> CheckedInTeamsKvpAsync(int tournamentId)
            => (await this.data
                .TeamsTournaments
                .Where(tt => tt.Id == tournamentId)
                .Select(t => new
                {
                    CheckedInTeamsId = t.RegisteredTeams
                        .Where(rt => rt.IsChecked)
                        .Select(rt => new IdNamePairTeamServiceModel
                        {
                            Id = rt.Id,
                            Name = rt.Team.Name
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync())
                .CheckedInTeamsId;

        public async Task<IEnumerable<TeamServiceModel>> CheckedInTeamsAsync(int tournamentId)
            => (await this.data
                .TeamsTournaments
                .Where(tt => tt.Id == tournamentId)
                .Select(t => new
                {
                    CheckedInTeams = t.RegisteredTeams
                        .Where(rt => rt.IsChecked)
                        .Select(rt => new TeamServiceModel
                        {
                            Id = rt.Id,
                            Name = rt.Team.Name,
                            Image = rt.Team.Appearance.Image,
                            Banner = rt.Team.Appearance.Banner,
                            RegistratedMembers = rt.InvitedMembers
                                .Select(m => new RegisteredMemberServiceModel
                                {
                                    TeamTournamentId = m.TeamsTournamentTeamId,
                                    UserId = m.UserId,
                                    IsChecked = m.IsChecked
                                })
                                .ToList(),
                            IsCheckedIn = rt.IsChecked,
                            IsEliminated = rt.IsEliminated
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync())
                .CheckedInTeams;

        public async Task<IEnumerable<TeamServiceModel>> TournamentParticipants(int tournamentId)
            => (await this.data
                .TeamsTournaments
                .Where(tt => tt.Id == tournamentId)
                .Select(tt => new
                {
                    Participants = tt.RegisteredTeams
                        .Select(rt => new TeamServiceModel
                        {
                            Id = rt.Team.Id,
                            Name = rt.Team.Name,
                            Image = rt.Team.Appearance.Image,
                            Banner = rt.Team.Appearance.Banner,
                            RegistratedMembers = rt.InvitedMembers
                                .Select(m => new RegisteredMemberServiceModel
                                {
                                    TeamTournamentId = m.TeamsTournamentTeamId,
                                    UserId = m.UserId,
                                    IsChecked = false
                                })
                                .AsEnumerable()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync())
                .Participants;

        public async Task<IEnumerable<RegisteredMemberServiceModel>> RegisteredMembersAsync(int tournamentTeamId)
            => (await this.data
                .TeamsTournamentsTeams
                .Where(ttt => ttt.Id == tournamentTeamId)
                .Select(ttt => new
                {
                    RegisteredMembers = ttt
                        .InvitedMembers
                        .Select(m => new RegisteredMemberServiceModel
                        {
                            UserId = m.UserId,
                            TeamTournamentId = m.TeamsTournamentTeamId,
                            IsChecked = m.IsChecked
                        })
                        .AsEnumerable()
                })
                .FirstOrDefaultAsync())
                .RegisteredMembers;

        public async Task<IEnumerable<RegisteredMemberServiceModel>> RegisteredMembersAsync(int tournamentId, int teamId)
            => await this.data
                .TeamsTournamentsTeams
                .Where(ttt => ttt.TeamsTournamentId == tournamentId && ttt.TeamId == teamId)
                .Select(ttt => ttt.InvitedMembers
                    .Select(m => new RegisteredMemberServiceModel
                    {
                        UserId = m.UserId,
                        TeamTournamentId = m.TeamsTournamentTeamId,
                        IsChecked = m.IsChecked
                    })
                    .AsEnumerable())
                .FirstOrDefaultAsync();

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

        public async Task<int> GetHosterIdAsync(string userId)
            => await this.data
                .Users
                .Where(u => u.Id == userId)
                .Select(u => u.HostTournaments.id)
                .FirstOrDefaultAsync();

        public async Task<string> GetInformationAsync(int tournamentId)
            => await this.data
                .TeamsTournaments
                .Where(tt => tt.Id == tournamentId)
                .Select(tt => tt.Information)
                .FirstOrDefaultAsync();

        public async Task AddInPending(
            string name,
            string information,
            DateTime startsOn,
            decimal prizePool,
            decimal ticketPrize,
            bool bronzeMatch,
            int minimumTeams,
            int goToGamePeriod,
            int regionId,
            int bracketId,
            int mapId,
            int maximumTeamsFormatId,
            int modeId,
            int teamSizeId,
            string userId)
        {

            var hosterId = await this.GetHosterIdAsync(userId);

            var tournament = new TeamsTournament
            {
                Name = name,
                Information = information,
                StartsOn = startsOn,
                PrizePool = prizePool,
                TicketPrize = ticketPrize,
                BronzeMatch = bronzeMatch,
                MinimumTeams = minimumTeams,
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

        public async Task RegisterTeam(int tournamentId, int teamId, IEnumerable<string> usersId)
        {
            var tournament = await this.data
                    .TeamsTournaments
                    .Where(tt => tt.Id == tournamentId)
                    .FirstOrDefaultAsync();

            var relation = new TeamsTournamentTeam
            {
                TeamId = teamId,
                TeamsTournamentId = tournamentId
            };

            await this.data.TeamsTournamentsTeams.AddAsync(relation);
            await this.data.SaveChangesAsync();

            //var teamTournament = this.data
            //    .TeamsTournamentsTeams
            //    .Where(ttt => ttt.TeamsTournamentId == tournamentId && ttt.TeamId == teamId)
            //    .FirstOrDefault();

            var members = new List<UserTeamsTournamentTeam>();

            foreach (var userId in usersId)
            {
                var member = new UserTeamsTournamentTeam
                {
                    TeamsTournamentTeamId = relation.Id,
                    UserId = userId
                };

                members.Add(member);
            }

            await this.data.UsersTeamsTournamentTeams.AddRangeAsync(members);
            await this.data.SaveChangesAsync();
        }

        public async Task Verify(int tournamentId)
        {
            var tournamentData = await this.data
                    .TeamsTournaments
                    .Where(tt => tt.Id == tournamentId)
                    .FirstOrDefaultAsync();

            tournamentData.IsVerified = true;

            await this.data.SaveChangesAsync();
        }

        public async Task Unverify(int tournamentId)
        {
            var tournamentData = await this.data
                    .TeamsTournaments
                    .Where(tt => tt.Id == tournamentId)
                    .FirstOrDefaultAsync();

            tournamentData.IsVerified = false;

            await this.data.SaveChangesAsync();
        }

        public async Task<bool> IsHosterAsync(string userId, string hosterName)
            => await this.data
                .TeamsTournaments
                .Where(tt => tt.Hoster.User.Nickname == hosterName)
                .AnyAsync(tt => tt.Hoster.User.Id == userId);

        public async Task<bool> BracketTypeExistsAsync(int bracketTypeId)
            => await this.data
            .BracketTypes
            .AnyAsync(bt => bt.Id == bracketTypeId);

        public async Task<bool> HasAlreadyStartedAsync(int tournamentId)
            => await this.data
                .TeamsTournaments
                .Where(tt => tt.Id == tournamentId)
                .Select(tt => tt.StartsOn > DateTime.UtcNow)
                .FirstOrDefaultAsync();

        public async Task<bool> IsFullAsync(int tournamentId)
            => await this.data
                    .TeamsTournaments
                    .Where(tt => tt.Id == tournamentId)
                    .Select(tt => tt.RegisteredTeams.Count > tt.MaximumTeamsFormat.Capacity)
                    .FirstOrDefaultAsync();

        public async Task<bool> IsTeamAlreadyRegisteredAsync(int tournamentId, int teamId)
            => await this.data
                .TeamsTournaments
                .Where(tt => tt.Id == tournamentId)
                .Select(tt => tt.RegisteredTeams.Any(rt => rt.TeamId == teamId))
                .FirstOrDefaultAsync();

        public async Task<bool> IsTeamCheckedAsync(int tournamentId, int teamId)
            => await this.data
                .TeamsTournamentsTeams
                .Where(ttt => ttt.TeamsTournamentId == tournamentId && ttt.TeamId == teamId)
                .Select(ttt => ttt.IsChecked)
                .FirstOrDefaultAsync();

        public async Task<int> GetTeamSizeAsync(int tournamentId)
        {
            var matchFormat = await this.data
                .TeamsTournaments
                .Where(tt => tt.Id == tournamentId)
                .Select(tt => tt.TeamSize)
                .FirstOrDefaultAsync();

            var teamSize = matchFormat.Format[0].ToString();

            return int.Parse(teamSize);
        }

        public async Task<bool> IsUserCheckedAsync(int tournamentId, int teamId, string userId)
            => await this.data
                .TeamsTournamentsTeams
                .Where(ttt => ttt.TeamsTournamentId == tournamentId && ttt.TeamId == teamId)
                .Select(ttt => ttt
                    .InvitedMembers
                    .Where(m => m.UserId == userId)
                    .Select(m => new
                    {
                        IsChecked = m.IsChecked
                    })
                    .FirstOrDefault()
                    .IsChecked)
                .FirstOrDefaultAsync();

        public async Task<bool> MapExistsAsync(int mapId)
            => await this.data
            .Maps
            .AnyAsync(m => m.Id == mapId);

        public async Task<bool> MaximumTeamsExistsAsync(int MaximumTeamsId)
            => await this.data
            .MaximumTeamsFormats
            .AnyAsync(mt => mt.Id == MaximumTeamsId);

        public async Task<bool> ModeExistsAsync(int modeId)
            => await this.data
            .Modes
            .AnyAsync(m => m.Id == modeId);

        public async Task<bool> TeamSizeExistsAsync(int teamSizeId)
            => await this.data
            .TeamSizes
            .AnyAsync(ts => ts.Id == teamSizeId);

        public async Task AddTournamentToHoster(string userId, TeamsTournament teamsTournament)
        {
            var hoster = await this.data
               .Users
               .Where(u => u.Id == userId)
               .Select(u => u.HostTournaments)
               .FirstOrDefaultAsync();

            hoster.TeamsTournaments.Add(teamsTournament);

            await this.data.SaveChangesAsync();
        }

        public async Task<IEnumerable<BracketTypeServiceModel>> AllBracketTypesAsync()
            => await this.data
                .BracketTypes
                .Select(bt => new BracketTypeServiceModel
                {
                    Id = bt.Id,
                    Name = bt.Name
                })
                .ToListAsync();

        public async Task<IEnumerable<MapServiceModel>> AllMapsAsync()
            => await this.data
                .Maps
                .Select(ts => new MapServiceModel
                {
                    Id = ts.Id,
                    Name = ts.Name
                })
                .ToListAsync();

        public async Task<IEnumerable<MaximumTeamsFormatServiceModle>> AllMaximumTeamsFormatsAsync()
            => await this.data
                .MaximumTeamsFormats
                .Select(mtf => new MaximumTeamsFormatServiceModle
                {
                    Id = mtf.Id,
                    Capacity = mtf.Capacity
                })
                .ToListAsync();

        public async Task<IEnumerable<ModeServiceModel>> AllModesAsync()
            => await this.data
                .Modes
                .Select(ts => new ModeServiceModel
                {
                    Id = ts.Id,
                    Name = ts.Name
                })
                .ToListAsync();

        public async Task<IEnumerable<TeamSizeServiceModel>> AllTeamSizesAsync()
            => await this.data
                .TeamSizes
                .Select(ts => new TeamSizeServiceModel
                {
                    Id = ts.Id,
                    Format = ts.Format
                })
                .ToListAsync();

        private bool IsValid(TournamentServiceModel tournament, int daysFrom, int daysTo, DateTime utcNow)
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

        private IEnumerable<TournamentServiceModel> OrderTournament(
            IEnumerable<TournamentServiceModel> tournament,
            string orderBy,
            string sorting)
        {

            if (sorting == "StartingDate")
            {
                if (orderBy == "date")
                {
                    return tournament.OrderByDescending(t => t.StartsOn);
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
                    return tournament.OrderBy(t => t.Name);
                }
                else
                {
                    return tournament.OrderBy(t => t.StartsOn);
                }
            }
            else
            {
                if (sorting == "Name")
                {
                    return tournament.OrderBy(t => t.Name);
                }
                else if (sorting == "TwoVsTwo")
                {
                    return tournament.OrderBy(t => t.TeamSizeFormat.Where(f => f.ToString() == "2v2"));
                }
                else if (sorting == "ThreeVsThree")
                {
                    return tournament.OrderBy(t => t.TeamSizeFormat.Where(f => f.ToString() == "3v3"));
                }
                else if (sorting == "FourVsFour")
                {
                    return tournament.OrderBy(t => t.TeamSizeFormat.Where(f => f.ToString() == "4v4"));
                }
                else if (sorting == "FiveVsFive")
                {
                    return tournament.OrderBy(t => t.TeamSizeFormat.Where(f => f.ToString() == "5v5"));
                }
                else
                {
                    return tournament.OrderByDescending(t => t.StartsOn);
                }
            }
        }

        private string ReturnEndingIfPlural(int quantity) => quantity == 1 ? string.Empty : "s";
    }
}