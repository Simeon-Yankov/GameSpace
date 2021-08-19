using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GameSpace.Services.Teams.Models;
using GameSpace.Services.Tournaments.Models;

using static GameSpace.Common.GlobalConstants.Tournament;

namespace GameSpace.Services.Tournaments.Contracts
{
    public interface ITournamentService
    {
        Task CheckInParticipant(int tournamentId, int teamId);

        TournamentServiceModel Details(int tournamentId);

        IEnumerable<TournamentServiceModel> AllUpcomingTournaments(
            int daysFromNow = default,
            int daysToNow = MaxDifferenceDaysInSchedule,
            string orderBy = "date",
            bool onlyVerified = false);

        IEnumerable<TeamServiceModel> TournamentParticipants(int tournamentId);

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

        Task RegisterTeam(int tournamentId, int teamId);

        Task Verify(int tournamentId);

        Task Unverify(int tournamentId);

        bool IsFull(int tournamentId);

        bool IsTeamAlreadyRegistrated(int tournamentId , int teamId);

        bool HasAlreadyStarted(int tournamentId);

        bool BracketTypeExists(int bracketTypeId);

        bool MapExists(int mapId);

        bool MaximumTeamsExists(int maximumTeamsId);

        bool ModeExists(int modeId);

        bool TeamSizeExists(int teamSizeId);

        IEnumerable<BracketTypeServiceModel> AllBracketTypes();

        IEnumerable<MapServiceModel> AllMaps();

        IEnumerable<MaximumTeamsFormatServiceModle> AllMaximumTeamsFormats();

        IEnumerable<ModeServiceModel> AllModes();

        IEnumerable<TeamSizeServiceModel> AllTeamSizes();
    }
}