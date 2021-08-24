using System.Collections.Generic;
using System.Linq;

using GameSpace.Data.Models;

namespace GameSpace.Test.Data
{
    public class TeamsTournaments
    {
        public static IEnumerable<TeamsTournament> TenTournamnets
            => Enumerable.Range(0, 10).Select(i => new TeamsTournament { });
    }
}