using System.Collections.Generic;
using System.Linq;

using GameSpace.Data.Models;

namespace GameSpace.Test.Data
{
    public class Regions
    {
        public static IEnumerable<Regions> TenRegions
            => Enumerable.Range(0, 10).Select(i => new Regions { });
    }
}