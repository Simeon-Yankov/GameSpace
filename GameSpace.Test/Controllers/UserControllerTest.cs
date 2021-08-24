using GameSpace.Controllers;
using GameSpace.Data.Models;
using GameSpace.Models.User;
using GameSpace.Services.Users.Models;

using MyTested.AspNetCore.Mvc;

using Xunit;

using GameSpace.Services.Appearances.Models;
using GameSpace.Models.SocialNetworks;

using static GameSpace.Test.TestConstants.User;

namespace GameSpace.Test.Controllers
{
    public class UserControllerTest
    {
        [Fact]
        public void GetProfileShouldBeAuthorizedUserAndReturnView()
            => MyController<UserController>
                .Instance()
                .WithData(data => data
                    .WithEntities(entities => entities.Add(
                        new User { Id = UserTestId })))
                .WithUser(UserTestId)
                .Calling(c => c.Profile(UserTestId))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<UserProfileServiceModel>());

        [Fact]
        public void GetEditShouldBeAuthorizedUserAndRedirect()
            => MyController<UserController>
                .Instance()
                .WithData(data => data
                    .WithEntities(entities => entities.Add(
                        new User { Id = UserTestId })))
                .WithUser(UserTestId)
                .Calling(c => c.Edit(UserTestId))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<EditUserFormModel>());

        [Fact]
        public void PostEditShouldBeAuthorizedUserAndRedirect()
            => MyController<UserController>
                .Instance()
                .WithData(data => data
                    .WithEntities(entities => entities.Add(
                        new User { Id = UserTestId })))
                .WithUser(UserTestId)
                .Calling(c => c.Edit(new EditUserFormModel() 
                {
                    Id = UserTestId,
                    Nickname = TestNickname,
                    Biography = "",
                    Appearance = new AppearanceServiceModel(),
                    SocialNetwork = new SocialNetworkViewModel()
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<UserController>(m => m.Profile(null)));
    }
}