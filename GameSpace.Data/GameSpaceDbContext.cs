using GameSpace.Data.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameSpace.Data
{
    public class GameSpaceDbContext : IdentityDbContext
    {
        public GameSpaceDbContext()
        {
        }

        public GameSpaceDbContext(DbContextOptions<GameSpaceDbContext> options)
           : base(options)
        {
        }

        public DbSet<Team> Teams { get; init; }

        public DbSet<SocialNetwork> SocialNetworks { get; init; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-OI0L4BE\SQLEXPRESS;Database=GameSpace;Integrated Security=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}