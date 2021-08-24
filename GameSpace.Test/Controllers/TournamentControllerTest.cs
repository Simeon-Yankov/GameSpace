using System.Collections.Generic;

using GameSpace.Controllers;
using GameSpace.Data.Models;
using GameSpace.Models.Teams;
using GameSpace.Models.Tournaments;
using GameSpace.Services.Teams.Models;

using MyTested.AspNetCore.Mvc;

using Xunit;

using static GameSpace.Test.TestConstants.Team;
using static GameSpace.Test.TestConstants.Tournament;
using static GameSpace.Test.TestConstants.User;

namespace GameSpace.Test.Controllers
{
    public class TournamentControllerTest
    {
        //[Fact]
        //public void GetCheckInShouldBeAuthorizedUserAndReturnView()
        //   => MyController<TournamentController>
        //       .Instance()
        //       .WithData(data => data
        //           .WithEntities(entities => entities.AddRange(
        //        new User { Id = UserTestId },
        //                new Team { Id = TestTeamId, OwnerId = UserTestId },
        //                new TeamsTournamentTeam()
        //                {
        //                    Id = 1,
        //                    TeamId = TestTeamId,
        //                    TeamsTournamentId = TournamentId,
        //                },
        //                new UserTeamsTournamentTeam
        //                {
        //                    UserId = UserTestId,
        //                    TeamsTournamentTeamId = 1
        //                },
        //                new TeamsTournament
        //                {
        //                    Id = TournamentId,
        //                    Name = Name,
        //                    RegionId = TestRegionId,
        //                    CheckInPeriod = 5,
        //                    GoToGamePeriod = 5,
        //                    MinimumTeams = 4,
        //                    BronzeMatch = true,
        //                    Region = new Region
        //                    {
        //                        Id = TestRegionId,
        //                        Name = TestRegionName,
        //                    },
        //                    BracketType = new BracketType(),
        //                    Map = new Map(),
        //                    Mode = new Mode(),
        //                    TeamSize = new TeamSize(),
        //                    Hoster = new HostTournaments(),
        //                    MaximumTeamsFormat = new MaximumTeamsFormat(),
        //                    BracketTypeId = 1,
        //                    ModeId = 1,
                            
        //                })))
        //       .WithUser(UserTestId)
        //       .Calling(c => c.CheckIn(TournamentId, TestRegionId))
        //       .ShouldHave()
        //       .ActionAttributes(attributes => attributes
        //           .RestrictingForAuthorizedRequests())
        //       .AndAlso()
        //       .ShouldReturn()
        //       .Redirect(redirect => redirect
        //            .To<HomeController>(m => m.Index()));

        //[Fact]
        //public void GetDetailsShouldBeAuthorizedUserAndReturnView()
        //   => MyController<TournamentController>
        //       .Instance()
        //       .WithData(data => data
        //           .WithEntities(entities => entities.AddRange(
        //                new Team { Id = TestTeamId, OwnerId = UserTestId },
        //                new TeamsTournament
        //                {
        //                    Id = TournamentId,
        //                    Name = Name,
        //                    Hoster = new HostTournaments() { User = new User { Id = UserTestId }, }
        //                }
        //                )))
        //       .WithUser(UserTestId)
        //       .Calling(c => c.Details(TournamentId))
        //       .ShouldHave()
        //       .ActionAttributes(attributes => attributes
        //           .RestrictingForAuthorizedRequests())
        //       .AndAlso()
        //       .ShouldReturn()
        //       .View(view => view
        //           .WithModelOfType<TournamentViewModel>());

        //    [Fact]
        //public void GetSelectionShouldBeAuthorizedUserAndReturnView()
        //   => MyController<TournamentController>
        //       .Instance()
        //       .WithData(data => data
        //           .WithEntities(entities => entities.AddRange(
        //               new User { Id = UserTestId },
        //               new Team { Id = TestTeamId, OwnerId = UserTestId },
        //               new TeamsTournament { Id = TournamentId }
        //               )))
        //       .WithUser(UserTestId)
        //       .Calling(c => c.Selection(TournamentId, TestTeamId))
        //       .ShouldHave()
        //       .ActionAttributes(attributes => attributes
        //           .RestrictingForAuthorizedRequests())
        //       .AndAlso()
        //       .ShouldReturn()
        //       .View(view => view
        //           .WithModelOfType<List<TournamentViewModel>>());

        [Fact]
        public void GetUpcomingShouldBeAuthorizedUserAndReturnView()
           => MyController<TournamentController>
               .Instance()
               .WithData(data => data
                   .WithEntities(entities => entities.AddRange(
                       new User { Id = UserTestId })))
               .WithUser(UserTestId)
               .Calling(c => c.Upcoming(new AllTournamentsQueryModel { }))
               .ShouldReturn()
               .View(view => view
                   .WithModelOfType<AllTournamentsQueryModel>());

        [Fact]
        public void GetCreateShouldBeAuthorizedUserAndReturnView()
           => MyController<TournamentController>
               .Instance()
               .WithData(data => data
                   .WithEntities(entities => entities.AddRange(
                       new User { Id = UserTestId })))
               .WithUser(UserTestId)
               .Calling(c => c.Create())
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .View(view => view
                   .WithModelOfType<CreateTournamentFormModel>());



    }
}