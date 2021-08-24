using System.Collections.Generic;

using GameSpace.Controllers;
using GameSpace.Data.Models;
using GameSpace.Models.Summoners;
using GameSpace.Services.Sumonners.Models;

using MyTested.AspNetCore.Mvc;

using Xunit;

using static GameSpace.Test.TestConstants.User;
using static GameSpace.Test.TestConstants.GameAccount;

namespace GameSpace.Test.Controllers
{
    public class SummonerControllerTest
    {
            [Fact]
        public void GetAddShouldBeAuthorizedUserAndReturnView()
            => MyController<SummonerController>
                .Instance(controller => controller
                    .WithUser())
                .Calling(c => c.Add())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<AddSummonerFormModel>());

        //TODO: API conflict with test
        //[Fact]
        //public void PostAddShouldBeAuthorizedUserAndReturnView()
        //            => MyController<SummonerController>
        //                .Instance()
        //                .WithData(data => data
        //                    .WithEntities(entities => entities.AddRange(
        //                        new User { Id = UserTestId },
        //                        new Region { Id = 1, Name = TestRegionName })))
        //                    .WithUser(UserTestId)
        //                .Calling(c => c.Add(new AddSummonerFormModel() 
        //                {
        //                    Name = "testName",
        //                    RegionId = 1,
        //                    Regions = new List<SummonerRegionServiceModel>()
        //                }))
        //                .ShouldHave()
        //                .ActionAttributes(attributes => attributes
        //                    .RestrictingForAuthorizedRequests())
        //                .AndAlso()
        //                .ShouldReturn()
        //                .Redirect(redirect => redirect
        //                    .To<UserController>(m => m.Profile(UserTestId)));

        [Fact]
        public void GetRefreshShouldBeAuthorizedUserAndRedirect()
            => MyController<SummonerController>
                .Instance()
                .WithData(data => data
                    .WithEntities(entities => entities.AddRange(
                        new User { Id = UserTestId },
                            new GameAccount { Id = 1, AccountId = AccountTestId })))
                .WithUser(UserTestId)
                .Calling(c => c.Refresh(new SummonerQueryModel 
                {
                    UserId = UserTestId,
                    AccountId = AccountTestId
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<UserController>(m => m.Profile(UserTestId)));

        [Fact]
        public void GetRemoveShouldBeAuthorizedUserAndRedirect()
            => MyController<SummonerController>
                .Instance()
                .WithData(data => data
                    .WithEntities(entities => entities.AddRange(
                        new User { Id = UserTestId },
                            new GameAccount { Id = 1, AccountId = AccountTestId })))
                    .WithUser(UserTestId)
                .Calling(c => c.Remove(AccountTestId))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<UserController>(m => m.Profile(null)));

        //[Fact]
        //public void GetVerifyShouldBeAuthorizedUserAndRedirect()
        //    => MyController<SummonerController>
        //        .Instance()
        //        .WithData(data => data
        //            .WithEntities(entities => entities.AddRange(
        //                new User { Id = UserTestId },
        //                    new GameAccount { Id = 1, AccountId = AccountTestId },
        //                    new Region { Id = 1, Name = TestRegionName})))
        //            .WithUser(UserTestId)
        //        .Calling(c => c.Verify(AccountTestId, TestRegionName))
        //        .ShouldHave()
        //        .ActionAttributes(attributes => attributes
        //            .RestrictingForAuthorizedRequests())
        //        .AndAlso()
        //        .ShouldReturn()
        //        .View(view => view
        //            .WithModelOfType<VerifySummonerServiceModel>());
        
    }
}