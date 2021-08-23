using System.Linq;

using GameSpace.Controllers;
using GameSpace.Data.Models;
using GameSpace.Models.Messages;

using MyTested.AspNetCore.Mvc;

using Xunit;

namespace GameSpace.Test.Controllers
{
    public class MessageControllerTest
    {
        [Fact]
        public void GetAllShouldBeAuthorizedUserAndReturnView()
            => MyController<MessageController>
                .Instance(controller => controller
                    .WithUser())
                .Calling(c => c.All())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<IOrderedEnumerable<MessageViewModel>>());

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