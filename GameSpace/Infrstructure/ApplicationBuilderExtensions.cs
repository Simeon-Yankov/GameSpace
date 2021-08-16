using System;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using GameSpace.Data;
using GameSpace.Data.Models;

using static GameSpace.Areas.Admin.AdminConstants;


namespace GameSpace.Infrstructure
{
    public static class ApplicationBuilderExtensions
    {

        public static async Task<IApplicationBuilder> PrepareDataBase(
            this IApplicationBuilder app)
        {
            using var scopeServices = app.ApplicationServices.CreateScope();
            var serviceProvider = scopeServices.ServiceProvider;

            MigrateDatabase(serviceProvider);

            await SeedLanguages(serviceProvider);
            await SeedRegions(serviceProvider);
            await SeedRanks(serviceProvider);
            await SeedBracketTypes(serviceProvider);
            await SeedMaximumTeamsFormats(serviceProvider);
            await SeedTeamSizes(serviceProvider);
            await SeedMaps(serviceProvider);
            await SeedModes(serviceProvider);
            await SeedAdministrator(serviceProvider);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<GameSpaceDbContext>();

            data.Database.Migrate();
        }

        private static async Task SeedLanguages(IServiceProvider services)
        {
            var data = services.GetRequiredService<GameSpaceDbContext>();

            if (data.Languages.Any())
            {
                return;
            }

            data.Languages.AddRange(new[]
            {
                new Language { Name = "English" },
                new Language { Name = "Bulgarian" },
                new Language { Name = "Greek" },
                new Language { Name = "French" },
                new Language { Name = "German" },
                new Language { Name = "Swedish" },
                new Language { Name = "Russian" },
                new Language { Name = "Turkish" },
                new Language { Name = "Polish" },
                new Language { Name = "Romanian" },
                new Language { Name = "Serbian" },
                new Language { Name = "Italian" },
                new Language { Name = "Spanish" },
                new Language { Name = "Portugal" }
            });

            await data.SaveChangesAsync();
        }

        private static async Task SeedRegions(IServiceProvider services)
        {
            var data = services.GetRequiredService<GameSpaceDbContext>();

            if (data.Regions.Any())
            {
                return;
            }

            data.Regions.AddRange(new[]
            {
                new Region { Name = "TR" },
                new Region { Name = "EUNE" },
                new Region { Name = "EUW" },
                new Region { Name = "NA" }
            });

            await data.SaveChangesAsync();
        }

        private static async Task SeedRanks(IServiceProvider services)
        {
            var data = services.GetRequiredService<GameSpaceDbContext>();

            if (data.Ranks.Any())
            {
                return;
            }

            data.Ranks.AddRange(new[]
            {
                new Rank { Name = "Iron" },
                new Rank { Name = "Bronze" },
                new Rank { Name = "Silver" },
                new Rank { Name = "Gold" },
                new Rank { Name = "Platinum" },
                new Rank { Name = "Diamond" },
                new Rank { Name = "Master" },
                new Rank { Name = "MasterGuardian" },
                new Rank { Name = "Challanger" }
            });

            await data.SaveChangesAsync();
        }

        private static async Task SeedBracketTypes(IServiceProvider services)
        {
            var data = services.GetRequiredService<GameSpaceDbContext>();

            if (data.BracketTypes.Any())
            {
                return;
            }

            data.BracketTypes.AddRange(new[]
            {
                new BracketType { Name = "Single Elimination" }
            });

            await data.SaveChangesAsync();
        }

        private static async Task SeedMaximumTeamsFormats(IServiceProvider services)
        {
            var data = services.GetRequiredService<GameSpaceDbContext>();

            if (data.MaximumTeamsFormats.Any())
            {
                return;
            }

            data.MaximumTeamsFormats.AddRange(new[]
            {
                new MaximumTeamsFormat { Capacity = 8 },
                new MaximumTeamsFormat { Capacity = 16 },
                new MaximumTeamsFormat { Capacity = 32 },
                new MaximumTeamsFormat { Capacity = 64 }
            });

            await data.SaveChangesAsync();
        }

        private static async Task SeedTeamSizes(IServiceProvider services)
        {
            var data = services.GetRequiredService<GameSpaceDbContext>();

            if (data.TeamSizes.Any())
            {
                return;
            }

            data.TeamSizes.AddRange(new[]
            {
                new TeamSize { Format = "5v5" },
                new TeamSize { Format = "4v4" },
                new TeamSize { Format = "3v3" },
                new TeamSize { Format = "2v2" }
            });

            await data.SaveChangesAsync();
        }

        private static async Task SeedMaps(IServiceProvider services)
        {
            var data = services.GetRequiredService<GameSpaceDbContext>();

            if (data.Maps.Any())
            {
                return;
            }

            data.Maps.AddRange(new[]
            {
                new Map { Name = "Howling Abyss" },
                new Map { Name = "Summoner's Rift" }
            });

            await data.SaveChangesAsync();
        }

        private static async Task SeedModes(IServiceProvider services)
        {
            var data = services.GetRequiredService<GameSpaceDbContext>();

            if (data.Modes.Any())
            {
                return;
            }

            data.Modes.AddRange(new[]
            {
                new Mode { Name = "Tournament Draft" },
                new Mode { Name = "All Random" },
                new Mode { Name = "Draft Mode" },
                new Mode { Name = "Blind Pick" }
            });

            await data.SaveChangesAsync();
        }

        private static async Task SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            if (await roleManager.RoleExistsAsync(AdministratorRoleName))
            {
                return;
            }

            var role = new IdentityRole { Name = AdministratorRoleName };

            await roleManager.CreateAsync(role);

            const string AdminEmail = "admin@gmail.com";
            const string AdminNickname = "Admin";
            const string AdminPassword = "GodLovesTheBraves";

            var user = new User
            {
                Email = AdminEmail,
                UserName = AdminEmail,
                Nickname = AdminNickname,
                CreatedOn = DateTime.UtcNow
            };

            await userManager.CreateAsync(user, AdminPassword);

            await userManager.AddToRoleAsync(user, role.Name);
        }
    }
}