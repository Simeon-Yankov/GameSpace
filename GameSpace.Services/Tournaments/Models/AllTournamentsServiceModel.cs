using System.Collections.Generic;

using GameSpace.Services.Tournaments.Models.Enum;

namespace GameSpace.Services.Tournaments.Models
{
    public class AllTournamentsServiceModel
    {
        public const int TournamentsPerPage = 3;

        public string SearchTerm { get; init; }

        public TournamentSorting Sorting { get; set; }

        public int CurrentPage { get; init; } = 1;

        public int TotalTournaments { get; set; }

        public IEnumerable<string> Brands { get; set; }

        public IEnumerable<TournamentServiceModel> Tournaments { get; set; }
    }
}