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

        TournamentServiceModel Details(int tournamentId);

        AllTournamentsServiceModel AllUpcomingTournaments(
            int daysFromNow = default,
            int daysToNow = MaxDifferenceDaysInSchedule,
            string orderBy = "date",
            bool onlyVerified = false,
            int currentPage = 1,
            int carsPerPage = int.MaxValue,
            string searchTerm = null,
            TournamentSorting sorting = 0);

        IEnumerable<IdNamePairTeamServiceModel> CheckedInTeamsKvp(int tournamentId);

        IEnumerable<TeamServiceModel> CheckedInTeams(int tournamentId);

        IEnumerable<TeamServiceModel> TournamentParticipants(int tournamentId);

        IEnumerable<RegisteredMemberServiceModel> RegisteredMembers(int tournamentTeamId);

        IEnumerable<RegisteredMemberServiceModel> RegisteredMembers(int tournamentId, int teamId);

        string GetBracketType(int bracketType);

        int GetCapacity(int capacityId);

        public string GetFormat(int formatId);

        string GetMapName(int mapId);

        string GetModeName(int modeId);

        string GetHosterName(int hosterId);

        int GetHosterId(string userId);

        string GetInformation(int id);

        string GetRegionName(int regionId);

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

        int GetTeamSize(int tournamentId);

        bool IsHoster(string userId, string hosterName);

        bool IsFull(int tournamentId);

        bool IsTeamAlreadyRegistered(int tournamentId , int teamId);

        bool IsTeamChecked(int tournamentId, int registeredTeamId);

        bool HasAlreadyStarted(int tournamentId);

        bool BracketTypeExists(int bracketTypeId);

        bool MapExists(int mapId);

        bool MaximumTeamsExists(int maximumTeamsId);

        bool ModeExists(int modeId);

        bool TeamSizeExists(int teamSizeId);

        bool IsUserChecked(int tournamentId, int teamId, string userId);

        IEnumerable<BracketTypeServiceModel> AllBracketTypes();

        IEnumerable<MapServiceModel> AllMaps();

        IEnumerable<MaximumTeamsFormatServiceModle> AllMaximumTeamsFormats();

        IEnumerable<ModeServiceModel> AllModes();

        IEnumerable<TeamSizeServiceModel> AllTeamSizes();
    }
}