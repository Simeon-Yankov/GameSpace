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

        public DbSet<GameAccount> GameAccounts { get; init; }

        public DbSet<Rank> Ranks { get; init; }

        public DbSet<Region> Regions { get; init; }

        public DbSet<ProfileInfoLanguage> ProfileInfosLanguages { get; init; }

        public DbSet<HostTournaments> HostsTournaments { get; init; }

        public DbSet<TeamsTournament> TeamsTournaments { get; init; }

        public DbSet<BracketType> BracketTypes { get; init; }

        public DbSet<MaximumTeamsFormat> MaximumTeamsFormats { get; init; }

        public DbSet<TeamSize> TeamSizes { get; init; }

        public DbSet<Map> Maps { get; init; }

        public DbSet<Mode> Modes { get; init; }

        public DbSet<Match> Matches { get; init; }

        public DbSet<TeamsTournamentTeam> TeamsTournamentsTeams { get; init; }

        public DbSet<UserTeamsTournamentTeam> UsersTeamsTournamentTeams { get; init; }

        public DbSet<API> APIs { get; init; }

        //public DbSet<Stat> Stats { get; init; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=DESKTOP-OI0L4BE\SQLEXPRESS;Database=GameSpace;Integrated Security=true;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTeam>(entity =>
            {
                entity.HasKey(ut => new { ut.UserId, ut.TeamId });
            });

            modelBuilder.Entity<ProfileInfoLanguage>(entity =>
            {
                entity.HasKey(pil => new { pil.ProfileInfoId, pil.LanguageId });
            });

            //modelBuilder.Entity<TeamsTournamentTeam>(entity =>
            //{
            //    entity.HasKey(ttt => new { ttt.TeamsTournamentId, ttt.TeamId });
            //});

            modelBuilder.Entity<UserTeamsTournamentTeam>(entity =>
            {
                entity.HasKey(uttt => new { uttt.TeamsTournamentTeamId, uttt.UserId });
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

            modelBuilder.Entity<GameAccount>()
                .HasOne(ga => ga.User)
                .WithMany(u => u.GameAccounts)
                .HasForeignKey(ga => ga.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeamsTournamentTeam>()
                .HasOne(tt => tt.TeamsTournament)
                .WithMany(ttt => ttt.RegisteredTeams)
                .HasForeignKey(ttt => ttt.TeamsTournamentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeamsTournamentTeam>()
                .HasOne(tt => tt.Team)
                .WithMany(t => t.Tournaments)
                .HasForeignKey(ttt => ttt.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserTeamsTournamentTeam>()
                .HasOne(tt => tt.User)
                .WithMany(t => t.InvitedToTournament)
                .HasForeignKey(ttt => ttt.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserTeamsTournamentTeam>()
                .HasOne(tt => tt.TeamsTournamentTeam)
                .WithMany(t => t.InvitedMembers)
                .HasForeignKey(ttt => ttt.TeamsTournamentTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}