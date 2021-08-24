using System.Collections.Generic;
using System.Linq;

using GameSpace.Data.Models;

namespace GameSpace.Test.Data
{
    public class GameAccounts
    {
        public static IEnumerable<GameAccount> gameGameAccount
            => Enumerable.Range(0, 10).Select(i => new GameAccount { });
    }
}