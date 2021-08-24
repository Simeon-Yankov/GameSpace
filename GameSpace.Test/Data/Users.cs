using System.Collections.Generic;
using System.Linq;

using GameSpace.Data.Models;

namespace GameSpace.Test.Data
{
    public class Users
    {
        public static IEnumerable<User> TwentyUsers
            => Enumerable.Range(0, 20).Select(i => new User { });
    }
}