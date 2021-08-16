using System;

namespace GameSpace.Services.Tournaments.Models
{
    public class TournamentServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Information { get; init; }

        public DateTime StartsOn { get; init; }

        public decimal PrizePool { get; init; }

        public decimal TicketPrize { get; init; }

        public int CheckInPeriod { get; init; }

        public int GoToGamePeriod { get; init; }

        public int RegionId { get; init; }

        public int BracketTypeId { get; init; }

        public int MaximumTeamsId { get; init; }

        public int MinimumTeams { get; init; }

        public int TeamSizeId { get; init; }

        public int MapId { get; init; }

        public int ModeId { get; init; }

        public bool BronzeMatch { get; init; }

        public bool IsVerified { get; init; }

        public int HosterId { get; init; }

        public string HosterName { get; init; }
    }
}