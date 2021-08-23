using System;
using System.Collections.Generic;

using GameSpace.Data.Models;
using GameSpace.Services.Appearances.Models;
using GameSpace.Services.SocialNetworks.Models;
using GameSpace.Services.Sumonners.Models;
using GameSpace.Services.Users.Contracts;
using GameSpace.Services.Users.Models;

using Moq;

using MyTested.AspNetCore.Mvc;

using static GameSpace.Test.TestConstants.User;

namespace GameSpace.Test.Mocks
{
    public class UserServiceMock
    {
        public static IUserService Instance
        {
            get
            {
                var mock = new Mock<IUserService>();

                var userProfileServiceModel = new UserProfileServiceModel
                {
                    Id = UserId,
                    Nickname = Nickname,
                    CreatedOn = new DateTime(2021, 8, 23),
                    Appearance = new AppearanceServiceModel(),
                    SocialNetwork = new SocialNotworkServiceModel(),
                    GameAccounts = new List<SummonerServiceModel>(),
                    Languages = new List<Language>()
                };

                mock
                    .Setup(us => us.Profile(With.Any<string>()))
                    .Returns(userProfileServiceModel);

                return mock.Object;
            }
        }
    }
}