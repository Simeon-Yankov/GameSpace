using GameSpace.Controllers;
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
        [InlineData(2)]
        public void GetClearShouldBeAuthorizedUserAndRedirect(
            int notificationId)
            => MyController<MessageController>
                .Instance(controller => controller
                    .WithUser())
                .Calling(c => c.Clear(notificationId))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<MessageController>(m => m.All()));
    }
}