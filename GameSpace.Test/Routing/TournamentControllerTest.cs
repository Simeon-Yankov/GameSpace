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
                .To<TournamentController>(c => c.CheckIn(1, 1));

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

        [Theory]
        [InlineData(TournamentId, TeamId)]
        public void GetSelectionShouldBeMapped(int tournamentId, int teamId)
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation("/Tournament/Selection")
                    .WithFormFields(new 
                    { 
                        TournamentId = tournamentId,
                        TeamId = teamId
                    }))
                .To<TournamentController>(c => c.Selection(tournamentId, teamId));
        //[Theory]
        //[InlineData(TeamId, TournamentId)]
        //public void GetSelection(int teamId, int tournamentId, IEnumerable<TeamMemberServiceModel> members = new List<TeamMemberServiceModel>())
        //    => MyRouting
        //        .Configuration()
        //        .ShouldMap(request => request
        //            .WithLocation("/Tournament/Selection")
        //            .WithFormFields(new 
        //            {
        //                TeamId = teamId,
        //                TournamentId = tournamentId,
        //                Members = members
        //            }))
        //        .To<TournamentController>(c => c.Selection(
        //            TournamentId,
        //            TeamId,
        //            new TeamMembersServiceModel
        //            {
        //                TeamId = teamId,
        //                TournamentId = tournamentId,
        //                Members = members
        //            }));

        [Fact]
        public void GetUpcomingShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tournament/Upcoming")
                .To<TournamentController>(c => c.Upcoming());

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