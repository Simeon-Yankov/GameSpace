using System;
using System.Collections.Generic;

using GameSpace.Data.Models;
using GameSpace.Services.Appearances.Models;
using GameSpace.Services.SocialNetworks.Models;
using GameSpace.Services.Sumonners.Models;

namespace GameSpace.Services.Users.Models
{
    public class UserProfileServiceModel
    {
        public string Id { get; init; }

        public string Nickname { get; init; }

        public string Biography { get; init; }

        public DateTime CreatedOn { get; init; }

        public AppearanceServiceModel Appearance { get; init; }

        public SocialNotworkServiceModel SocialNetwork { get; init; }

        public IEnumerable<SummonerServiceModel> GameAccounts { get; init; }

        public IEnumerable<Language> Languages { get; init; }

    }
}