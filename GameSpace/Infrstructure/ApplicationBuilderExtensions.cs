using System;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using GameSpace.Data;
using GameSpace.Data.Models;

using static GameSpace.WebConstants;


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