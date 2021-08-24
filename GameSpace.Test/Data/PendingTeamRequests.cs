using System.Collections.Generic;
using System.Linq;

using GameSpace.Data.Models;

namespace GameSpace.Test.Data
{
    public class PendingTeamRequests
    {
        public static IEnumerable<PendingTeamRequest> TenPendingTeamRequests
            => Enumerable.Range(0, 10).Select(i => new PendingTeamRequest { });
    }
}