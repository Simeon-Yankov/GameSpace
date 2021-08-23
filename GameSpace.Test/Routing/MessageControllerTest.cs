using GameSpace.Controllers;

using MyTested.AspNetCore.Mvc;

using Xunit;

namespace GameSpace.Test.Routing
{
    public class MessageControllerTest
    {
        [Fact]
        public void GetAllShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Message/All")
                .To<MessageController>(c => c.All());

        [Fact]
        public void GetClearShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap( "/Message/Clear")
                .To<MessageController>(c => c.Clear(With.Any<int>()));
    }
}