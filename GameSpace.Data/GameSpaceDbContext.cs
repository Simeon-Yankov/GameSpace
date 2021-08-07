using GameSpace.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameSpace.Data
{
    public class GameSpaceDbContext : IdentityDbContext<User>
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

        public DbSet<UserTeam> UsersTeams { get; init; }

        public DbSet<Appearance> Appearances { get; init; }

        public DbSet<PendingTeamRequest> PendingTeamsRequests { get; init; }

        public DbSet<Notification> Notifications { get; init; }

        public DbSet<ProfileInfo> ProfilesInfo { get; init; }

        public DbSet<Language> Languages { get; init; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-OI0L4BE\SQLEXPRESS;Database=GameSpace;Integrated Security=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTeam>(entity =>
            {
                entity.HasKey(ut => new { ut.UserId, ut.TeamId });
            });

            //modelBuilder.Entity<UserTeam>()
            //    .HasOne<User>()
            //    .WithMany()
            //    .HasForeignKey(ut => ut.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserTeam>()
                .HasOne(ut => ut.Team)
                .WithMany(t => t.Mombers)
                .HasForeignKey(ut => ut.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}