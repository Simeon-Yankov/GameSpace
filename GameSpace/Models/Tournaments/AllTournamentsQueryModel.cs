using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using GameSpace.Services.Tournaments.Models.Enum;

namespace GameSpace.Models.Tournaments
{
    public class AllTournamentsQueryModel
    {
        public const int TournamentsPerPage = 3;

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public TournamentSorting Sorting { get; set; }

        public int CurrentPage { get; init; } = 1;

        public int TotalTournaments { get; set; }

        public IEnumerable<string> Brands { get; set; }

        public IEnumerable<TournamentViewModel> Tournaments { get; init; }
    }
}