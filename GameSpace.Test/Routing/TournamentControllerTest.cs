using GameSpace.Controllers;
using GameSpace.Models.Tournaments;
using GameSpace.Services.Teams.Models;

using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;

using static GameSpace.Test.TestConstants.Team;
using static GameSpace.Test.TestConstants.Tournament;

namespace GameSpace.Test.Routing
{
    public class TournamentControllerTest
    {
        [Fact]
        public void GetCheckInShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tournament/CheckIn")
                .To<TournamentController>(c => c.CheckIn(With.Any<int>(), With.Any<int>()));

        [Fact]
        public void GetDetailsShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tournament/Details")
                .To<TournamentController>(c => c.Details(With.Any<int>()));

        [Fact]
        public void GetParticipationShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tournament/Participation")
                .To<TournamentController>(c => c.Participation(With.Any<int>()));

        [Fact]
        public void GetSelectionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tournament/Selection")
                .To<TournamentController>(c => c.Selection(With.Any<int>(), With.Any<int>()));

        [Fact]
        public void PostSelectionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(map => map
                    .WithLocation("/Tournament/Selection")
                    .WithMethod(HttpMethod.Post))
                .To<TournamentController>(c => c.Selection(
                    With.Any<int>(),
                    With.Any<int>(),
                    With.Any<TeamMembersServiceModel>()));

        [Fact]
        public void GetUpcomingShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tournament/Upcoming")
                .To<TournamentController>(c => c.Upcoming(new AllTournamentsQueryModel { }));

        [Fact]
        public void GetCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tournament/Create")
                .To<TournamentController>(c => c.Create());

        [Fact]
        public void PostCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(map => map
                    .WithPath("/Tournament/Create")
                    .WithMethod(HttpMethod.Post))
                .To<TournamentController>(c => c.Create(With.Any<CreateTournamentFormModel>()));


    }
}