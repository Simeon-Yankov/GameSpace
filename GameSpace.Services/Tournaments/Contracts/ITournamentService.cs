using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GameSpace.Services.Teams.Models;
using GameSpace.Services.Tournaments.Models;
using GameSpace.Services.Tournaments.Models.Enum;
using static GameSpace.Common.GlobalConstants.Tournament;

namespace GameSpace.Services.Tournaments.Contracts
{
    public interface ITournamentService
    {
        Task CheckInParticipant(int tournamentId, int teamId, string userId);

        Task<TournamentServiceModel> DetailsAsync(int tournamentId);

        Task<AllTournamentsServiceModel> AllUpcomingTournamentsAsync(
            int daysFromNow = default,
            int daysToNow = MaxDifferenceDaysInSchedule,
            string orderBy = "date",
            bool onlyVerified = false,
            int currentPage = 1,
            int carsPerPage = int.MaxValue,
            string searchTerm = null,
            TournamentSorting sorting = 0);

        Task<IEnumerable<IdNamePairTeamServiceModel>> CheckedInTeamsKvpAsync(int tournamentId);

        Task<IEnumerable<TeamServiceModel>> CheckedInTeamsAsync(int tournamentId);

        Task<IEnumerable<TeamServiceModel>> TournamentParticipants(int tournamentId);

        Task<IEnumerable<RegisteredMemberServiceModel>> RegisteredMembersAsync(int tournamentTeamId);

        Task<IEnumerable<RegisteredMemberServiceModel>> RegisteredMembersAsync(int tournamentId, int teamId);

        Task<int> GetHosterIdAsync(string userId);

        Task<string> GetInformationAsync(int id);

        Task AddInPending(
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
            string userId);

        Task RegisterTeam(int tournamentId, int teamId, IEnumerable<string> usersId);

        Task Verify(int tournamentId);

        Task Unverify(int tournamentId);

        Task<int> GetTeamSizeAsync(int tournamentId);

        Task<bool> IsHosterAsync(string userId, string hosterName);

        Task<bool> IsFullAsync(int tournamentId);

        Task<bool> IsTeamAlreadyRegisteredAsync(int tournamentId , int teamId);

        Task<bool> IsTeamCheckedAsync(int tournamentId, int registeredTeamId);

        Task<bool> HasAlreadyStartedAsync(int tournamentId);

        Task<bool> BracketTypeExistsAsync(int bracketTypeId);

        Task<bool> MapExistsAsync(int mapId);

        Task<bool> MaximumTeamsExistsAsync(int maximumTeamsId);

        Task<bool> ModeExistsAsync(int modeId);

        Task<bool> TeamSizeExistsAsync(int teamSizeId);

        Task<bool> IsUserCheckedAsync(int tournamentId, int teamId, string userId);

        Task<IEnumerable<BracketTypeServiceModel>> AllBracketTypesAsync();

        Task<IEnumerable<MapServiceModel>> AllMapsAsync();

        Task<IEnumerable<MaximumTeamsFormatServiceModle>> AllMaximumTeamsFormatsAsync();

        Task<IEnumerable<ModeServiceModel>> AllModesAsync();

        Task<IEnumerable<TeamSizeServiceModel>> AllTeamSizesAsync();
    }
}