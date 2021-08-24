using System.Collections.Generic;
using System.Linq;

using GameSpace.Data.Models;

namespace GameSpace.Test.Data
{
    public class UsersTeamsTournamentsTeams
    {
        public static IEnumerable<TeamsTournamentTeam> TenTeamsTournamentTeam
            => Enumerable.Range(0, 10).Select(i => new TeamsTournamentTeam { });
    }
}