using GameSpace.Controllers;
using GameSpace.Models.User;
using GameSpace.Services.Users.Models;

using MyTested.AspNetCore.Mvc;

using Xunit;

using static GameSpace.Test.TestConstants.User;

namespace GameSpace.Test.Controllers
{
    public class UserControllerTest
    {
        [Theory]
        [InlineData(UserId)]
        public void GetProfileShouldBeAuthorizedUserAndReturnView(
            string userId)
            => MyController<UserController>
                .Instance(controller => controller
                    .WithUser())
                .Calling(c => c.Profile(userId))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<UserProfileServiceModel>());

        [Theory]
        [InlineData("TestData")]
        public void GetEditShouldBeAuthorizedUserAndRedirect(
            string id)
            => MyController<UserController>
                .Instance(controller => controller
                    .WithUser())
                .Calling(c => c.Edit(id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<EditUserFormModel>());

        [Theory]
        [InlineData("TestData")]
        public void PostEditShouldBeAuthorizedUserAndRedirect(
            string id)
            => MyController<UserController>
                .Instance(controller => controller
                    .WithUser())
                .Calling(c => c.Edit(id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<EditUserFormModel>());
    }
}