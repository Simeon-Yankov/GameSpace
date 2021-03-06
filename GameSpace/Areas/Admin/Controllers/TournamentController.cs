using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using GameSpace.Areas.Admin.Models;
using GameSpace.Models.Tournaments;
using GameSpace.Services.Tournaments.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace GameSpace.Areas.Admin.Controllers
{
    public class TournamentController : AdminController
    {
        private readonly ITournamentService tournaments;
        private readonly IMapper mapper;

        public TournamentController(ITournamentService tournaments, IMapper mapper)
        {
            this.tournaments = tournaments;
            this.mapper = mapper;
        }

        public IActionResult All(string orderBy = "date")
        {
            var tournamentsData = this.tournaments.AllUpcomingTournaments(orderBy: orderBy);

            var tournamentsForm = mapper.Map<AllTournamentsQueryModel>(tournamentsData);

            return View(tournamentsForm);
        }

        public IActionResult Information(int tournamentId)
        {
            var informationData = this.tournaments.GetInformation(tournamentId);

            return View(new InformationViewModel 
            {
                Information = informationData
            });
        }

        public async Task<IActionResult> Verify(int tournamentId)
        {
            await this.tournaments.Verify(tournamentId);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> UnVerify(int tournamentId)
        {
            await this.tournaments.Unverify(tournamentId);

            return RedirectToAction(nameof(All));
        }
    }
}