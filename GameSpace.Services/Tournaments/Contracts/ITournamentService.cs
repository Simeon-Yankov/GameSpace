using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GameSpace.Services.Tournaments.Models;

using static GameSpace.Common.GlobalConstants.Tournament;

namespace GameSpace.Services.Tournaments.Contracts
{
    public interface ITournamentService
    {
        TournamentServiceModel Details(int tournamentId);

        IEnumerable<TournamentServiceModel> AllUpcomingTournaments(
            int daysFromNow = default,
            int daysToNow = MaxDifferenceDaysInSchedule,
            string orderBy = "date",
            bool onlyVerified = false);

        string GetBracketType(int bracketType);

        public string GetFormat(int formatId);

        int GetCapacity(int capacityId);

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
            int checkInPeriod,
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