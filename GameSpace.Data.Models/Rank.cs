using System.Collections.Generic;

namespace GameSpace.Data.Models
{
    public class Rank
    {
        public Rank() 
            => this.GameAccounts = new HashSet<GameAccount>();

        public int Id { get; init; }

        public string Name { get; set; }

        public virtual ICollection<GameAccount> GameAccounts { get; init; }
    }
}