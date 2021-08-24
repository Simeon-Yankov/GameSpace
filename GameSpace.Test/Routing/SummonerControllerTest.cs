using GameSpace.Controllers;
using GameSpace.Models.Summoners;
using GameSpace.Services.Sumonners.Models;

using MyTested.AspNetCore.Mvc;

using Xunit;

using static GameSpace.Test.TestConstants.GameAccount;

namespace GameSpace.Test.Routing
{
    public class SummonerControllerTest
    {
        [Fact]
        public void GetAddShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Summoner/Add")
                .To<SummonerController>(c => c.Add());

        [Fact]
        public void PostAddShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(map => map
                    .WithPath("/Summoner/Add")
                    .WithMethod(HttpMethod.Post))
                .To<SummonerController>(c => c.Add(With.Any<AddSummonerFormModel>()));

        [Fact]
        public void GetRefreshShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Summoner/Refresh")
                .To<SummonerController>(c => c.Refresh(With.Any<SummonerQueryModel>()));

        [Fact]
        public void GetRemoveShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Summoner/Remove")
                .To<SummonerController>(c => c.Remove(With.Any<string>()));

        [Fact]
        public void GetVerifyShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(map => map
                    .WithPath("/Summoner/Verify")
                    .WithMethod(HttpMethod.Get))
                .To<SummonerController>(c => c.Verify(With.Any<string>(), With.Any<string>()));

        [Fact]
        public void PostVerifyShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(map => map
                    .WithPath("/Summoner/Verify")
                    .WithMethod(HttpMethod.Post))
                .To<SummonerController>(c => c.Verify(With.Any<VerifySummonerServiceModel>()));
    }
}