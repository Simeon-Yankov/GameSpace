using System.Collections.Generic;

using GameSpace.Controllers;
using GameSpace.Data.Models;
using GameSpace.Models.Teams;
using GameSpace.Services.Teams.Models;

using MyTested.AspNetCore.Mvc;

using Xunit;

using static GameSpace.Test.TestConstants.Team;
using static GameSpace.Test.TestConstants.User;
using static GameSpace.Test.TestConstants.PendingRequest;


namespace GameSpace.Test.Controllers
{
    public class TeamControllerTest
    {
        [Fact]
        public void GetDetailsShouldBeAuthorizedUserAndReturnView()
           => MyController<TeamController>
               .Instance()
               .WithData(data => data
                   .WithEntities(entities => entities.AddRange(
                        new User { Id = UserTestId },
                        new Team
                        {
                            Id = TestTeamId,
                            OwnerId = UserTestId
                        })))
               .WithUser(UserTestId)
               .Calling(c => c.Details(TestTeamId, UserTestId))
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .View(view => view
                   .WithModelOfType<TeamDetailsServiceModel>());

        [Fact]
        public void GetInviteShouldBeAuthorizedUserAndReturnView()
           => MyController<TeamController>
               .Instance()
               .WithData(data => data
                   .WithEntities(entities => entities.AddRange(
                        new User { Id = UserTestId },
                        new Team
                        {
                            Id = TestTeamId,
                            OwnerId = UserTestId
                        })))
               .WithUser(UserTestId)
               .Calling(c => c.Invite(TestTeamId))
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .View(view => view
                   .WithModelOfType<InviteTeamFormModel>());

        [Fact]
        public void PostInviteShouldBeAuthorizedUserAndReturnView()
           => MyController<TeamController>
               .Instance()
               .WithData(data => data
                   .WithEntities(entities => entities
                   .AddRange(
                new User
                {
                    Id = UserTestId,
                    Nickname = TestNickname
                },
                        new User
                        {
                            Id = UserTestIdSecond,
                            Nickname = TestNicknameSecond
                        },
                        new Team
                        {
                            Id = TestTeamId,
                            OwnerId = UserTestId,
                            Mombers = new HashSet<UserTeam>()
                            {
                                new UserTeam()
                                {
                                    UserId = UserTestId,
                                    TeamId = TestTeamId
                                }
                            }
                        })))
               .WithUser(UserTestId)
               .Calling(c => c.Invite(new InviteTeamFormModel
               {
                   TeamId = TestTeamId,
                   Nickname = TestNicknameSecond
               }))
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .Redirect(redirect => redirect
                    .To<TeamController>(m => m.Memberships()));

        [Fact]
        public void GetAcceptInvitationShouldBeAuthorizedUserAndReturnView()
           => MyController<TeamController>
               .Instance()
               .WithData(data => data
                   .WithEntities(entities => entities
                   .AddRange(
                new User
                {
                    Id = UserTestId,
                    Nickname = TestNickname
                },
                        new PendingTeamRequest
                        {
                            Id = PendingRequestId
                        })))
               .WithUser(UserTestId)
               .Calling(c => c.AcceptInvitation(PendingRequestId))
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .Redirect(redirect => redirect
                    .To<MessageController>(m => m.All()));

        [Fact]
        public void GetDeclineInvitationShouldBeAuthorizedUserAndReturnView()
           => MyController<TeamController>
               .Instance()
               .WithData(data => data
                   .WithEntities(entities => entities
                   .AddRange(
                new User
                {
                    Id = UserTestId,
                    Nickname = TestNickname
                },
                        new PendingTeamRequest
                        {
                            Id = PendingRequestId
                        })))
               .WithUser(UserTestId)
               .Calling(c => c.DeclineInvitation(PendingRequestId))
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .Redirect(redirect => redirect
                    .To<MessageController>(m => m.All()));

        //[Fact]
        //public void GetMembersShouldBeAuthorizedUserAndReturnView()
        //   => MyController<TeamController>
        //       .Instance()
        //       .WithData(data => data
        //           .WithEntities(entities => entities
        //           .AddRange(
        //        new User
        //                {
        //                    Id = UserTestId,
        //                    Nickname = TestNickname,

        //                },
        //                new Team
        //                {
        //                    Id = TestTeamId,
        //                    OwnerId = UserTestId,
        //                    Mombers = new HashSet<UserTeam>()
        //                    {
        //                        new UserTeam()
        //                        {
        //                            UserId = UserTestId,
        //                            TeamId = TestTeamId
        //                        }
        //                    }
        //                })))
        //       .WithUser(UserTestId)
        //       .Calling(c => c.Members(PendingRequestId))
        //       .ShouldHave()
        //       .ActionAttributes(attributes => attributes
        //           .RestrictingForAuthorizedRequests())
        //       .AndAlso()
        //       .ShouldReturn()
        //       .View(view => view
        //           .WithModelOfType<TeamMembersServiceModel>());

        [Fact]
        public void GetLeaveShouldBeAuthorizedUserAndReturnView()
       => MyController<TeamController>
           .Instance()
           .WithData(data => data
               .WithEntities(entities => entities
               .AddRange(
            new User
                    {
                        Id = UserTestId,
                        Nickname = TestNickname,
                    },
                    new User
                    {
                        Id = UserTestIdSecond,
                        Nickname = TestNicknameSecond,
                    },
                    new Team
                    {
                        Id = TestTeamId,
                        OwnerId = UserTestId,
                        Mombers = new HashSet<UserTeam>()
                        {
                            new UserTeam()
                            {
                                UserId = UserTestId,
                                TeamId = TestTeamId
                            },
                            new UserTeam()
                            {
                                UserId = TestNicknameSecond,
                                TeamId = TestTeamId
                            }
                        }
                    })))
           .WithUser(UserTestId)
           .Calling(c => c.Leave(TestTeamId, TestNicknameSecond))
           .ShouldHave()
           .ActionAttributes(attributes => attributes
               .RestrictingForAuthorizedRequests())
           .AndAlso()
           .ShouldReturn()
           .Redirect(redirect => redirect
                    .To<TeamController>(m => m.Memberships()));

        [Fact]
        public void GetPromoteToOwnerShouldBeAuthorizedUserAndReturnView()
       => MyController<TeamController>
           .Instance()
           .WithData(data => data
               .WithEntities(entities => entities
               .AddRange(
            new User
                    {
                        Id = UserTestId,
                        Nickname = TestNickname,
                    },
                    new User
                    {
                        Id = UserTestIdSecond,
                        Nickname = TestNicknameSecond,
                    },
                    new Team
                    {
                        Id = TestTeamId,
                        OwnerId = UserTestId,
                        Mombers = new HashSet<UserTeam>()
                        {
                            new UserTeam()
                            {
                                UserId = UserTestId,
                                TeamId = TestTeamId
                            },
                            new UserTeam()
                            {
                                UserId = TestNicknameSecond,
                                TeamId = TestTeamId
                            }
                        }
                    })))
           .WithUser(UserTestId)
           .Calling(c => c.PromoteToOwner(TestTeamId, TestNicknameSecond))
           .ShouldHave()
           .ActionAttributes(attributes => attributes
               .RestrictingForAuthorizedRequests())
           .AndAlso()
           .ShouldReturn()
           .Redirect(redirect => redirect
                    .To<TeamController>(m => m.Memberships()));

        [Fact]
        public void GetCreateShouldBeAuthorizedUserAndReturnView()
        => MyController<TeamController>
            .Instance(controller => controller
                .WithUser())
            .Calling(c => c.Create())
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View();

        [Fact]
        public void PostCreateShouldBeAuthorizedUserAndReturnView()
       => MyController<TeamController>
           .Instance()
           .WithData(data => data
               .WithEntities(entities => entities
               .AddRange(
            new User
                    {
                        Id = UserTestId,
                        Nickname = TestNickname,
                    })))
           .WithUser(UserTestId)
           .Calling(c => c.Create(new AddServiceModel 
           { 
                Name = TestTeamName
           },
               null))
           .ShouldHave()
           .ActionAttributes(attributes => attributes
               .RestrictingForAuthorizedRequests())
           .AndAlso()
           .ShouldReturn()
           .Redirect(redirect => redirect
                    .To<HomeController>(m => m.Index()));

        [Fact]
        public void GetEditShouldBeAuthorizedUserAndReturnView()
       => MyController<TeamController>
           .Instance()
           .WithData(data => data
               .WithEntities(entities => entities
               .AddRange(
        new User
                {
                    Id = UserTestId,
                    Nickname = TestNickname,
                },
                new Team
                {
                    Id = TestTeamId,
                    OwnerId = UserTestId,
                    Mombers = new HashSet<UserTeam>()
                    {
                        new UserTeam()
                        {
                            UserId = UserTestId,
                            TeamId = TestTeamId
                        }
                    }
                }
            )))
           .WithUser(UserTestId)
           .Calling(c => c.Edit(TestTeamId))
           .ShouldHave()
           .ActionAttributes(attributes => attributes
               .RestrictingForAuthorizedRequests())
           .AndAlso()
           .ShouldReturn()
           .View(view => view
                   .WithModelOfType<EditTeamFromModel>());

        [Fact]
        public void PostEditShouldBeAuthorizedUserAndReturnView()
       => MyController<TeamController>
           .Instance()
           .WithData(data => data
               .WithEntities(entities => entities
               .AddRange(
        new User
        {
            Id = UserTestId,
            Nickname = TestNickname,
        },
                new Team
                {
                    Id = TestTeamId,
                    OwnerId = UserTestId,
                    Mombers = new HashSet<UserTeam>()
                    {
                        new UserTeam()
                        {
                            UserId = UserTestId,
                            TeamId = TestTeamId
                        }
                    }
                }
            )))
           .WithUser(UserTestId)
           .Calling(c => c.Edit(new EditTeamFromModel() { Id = TestTeamId, Name = TestTeamName}))
           .ShouldHave()
           .ActionAttributes(attributes => attributes
               .RestrictingForAuthorizedRequests())
           .AndAlso()
           .ShouldReturn()
           .Redirect(redirect => redirect
                    .To<HomeController>(m => m.Index()));

        [Fact]
        public void GetDeleteShouldBeAuthorizedUserAndReturnView()
       => MyController<TeamController>
           .Instance()
           .WithData(data => data
               .WithEntities(entities => entities
               .AddRange(
        new User
                {
                    Id = UserTestId,
                    Nickname = TestNickname,
                },
                new Team
                {
                    Id = TestTeamId,
                    OwnerId = UserTestId,
                    Mombers = new HashSet<UserTeam>()
                    {
                        new UserTeam()
                        {
                            UserId = UserTestId,
                            TeamId = TestTeamId
                        }
                    }
                }
            )))
           .WithUser(UserTestId)
           .Calling(c => c.Delete(TestTeamId))
           .ShouldHave()
           .ActionAttributes(attributes => attributes
               .RestrictingForAuthorizedRequests())
           .AndAlso()
           .ShouldReturn()
           .Redirect(redirect => redirect
                    .To<TeamController>(m => m.Memberships()));
    }
}