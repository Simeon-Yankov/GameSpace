using GameSpace.Controllers;
using GameSpace.Models.Teams;
using GameSpace.Services.Teams.Models;

using Microsoft.AspNetCore.Http;

using MyTested.AspNetCore.Mvc;

using Xunit;

using static GameSpace.Test.TestConstants.Team;
using static GameSpace.Test.TestConstants.User;


namespace GameSpace.Test.Routing
{
    public class TeamControllerTest
    {
        [Fact]
        public void GetDetailsShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Team/Details")
                .To<TeamController>(c => c.Details(With.Any<int>()));

        [Fact]
        public void GetInviteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Team/Invite")
                .To<TeamController>(c => c.Invite(With.Any<int>()));

        [Fact]
        public void PostInviteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(map => map
                    .WithPath("/Team/Invite")
                    .WithMethod(HttpMethod.Post))
                .To<TeamController>(c => c.Invite(With.Any<InviteTeamFormModel>()));

        [Fact]
        public void GetAcceptInvitationShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Team/AcceptInvitation")
                .To<TeamController>(c => c.AcceptInvitation(With.Any<int>()));

        [Fact]
        public void GetDeclineInvitationShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Team/DeclineInvitation")
                .To<TeamController>(c => c.DeclineInvitation(With.Any<int>()));

        [Fact]
        public void GetMembershipsShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Team/Memberships")
                .To<TeamController>(c => c.Memberships());

        [Fact]
        public void GetMembersShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Team/Members")
                .To<TeamController>(c => c.Members(With.Any<int>()));

        [Fact]
        public void GetLeaveShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Team/Leave")
                .To<TeamController>(c => c.Leave(TeamId, null));

        //[Theory]
        //[InlineData(TeamId, UserId)]
        //public void GetLeaveShouldBeRoutedCorrectly(int teamId, string userId)
        //    => MyRouting
        //        .Configuration()
        //        .ShouldMap(request => request
        //            .WithMethod(HttpMethod.Post)
        //            .WithLocation("/Team/Leave")
        //            .WithFormFields(new
        //            {
        //                TeamId = teamId,
        //                UserId = userId
        //            }))
        //        .To<TeamController>(c => c.Leave(TeamId, UserId));


        [Fact]
        public void GetPromoteToOwnerShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Team/PromoteToOwner")
                .To<TeamController>(c => c.PromoteToOwner(TeamId, UserId));

        [Fact]
        public void GetCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Team/Create")
                .To<TeamController>(c => c.Create());

        [Fact]
        public void PostCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(map => map
                    .WithPath("/Team/Create")
                    .WithMethod(HttpMethod.Post))
                .To<TeamController>(c => c.Create(new AddServiceModel
                {
                    Name = TeamName
                },
                    With.Any<IFormFile>()));

        [Fact]
        public void GetEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Team/Edit")
                .To<TeamController>(c => c.Edit(With.Any<int>()));

        [Fact]
        public void PostEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(map => map
                    .WithPath("/Team/Edit")
                    .WithMethod(HttpMethod.Post))
                .To<TeamController>(c => c.Edit(With.Any<EditTeamFromModel>()));

        [Fact]
        public void GetDeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Team/Delete")
                .To<TeamController>(c => c.Delete(With.Any<int>()));
    }
}