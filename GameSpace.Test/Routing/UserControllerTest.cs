using GameSpace.Controllers;
using GameSpace.Models.User;

using MyTested.AspNetCore.Mvc;

using Xunit;

namespace GameSpace.Test.Routing
{
    public class UserControllerTest
    {
        [Fact]
        public void GetProfileShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/User/Profile")
                .To<UserController>(c => c.Profile(With.Any<string>()));

        [Fact]
        public void GetEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/User/Edit")
                .To<UserController>(c => c.Edit(With.Any<string>()));

        [Fact]
        public void PostEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(map => map
                    .WithPath("/User/Edit")
                    .WithMethod(HttpMethod.Post))
                .To<UserController>(c => c.Edit(With.Any<EditUserFormModel>()));
    }
}