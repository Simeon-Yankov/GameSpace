using System.Collections.Generic;
using System.Linq;

using GameSpace.Data.Models;

namespace GameSpace.Test.Data
{
    public class Teams
    {
        public static IEnumerable<Team> TenTeams
            => Enumerable.Range(0, 10).Select(i => new Team { });
    }
}