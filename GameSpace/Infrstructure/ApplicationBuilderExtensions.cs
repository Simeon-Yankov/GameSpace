using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using GameSpace.Data;
using GameSpace.Data.Models;

using static GameSpace.WebConstants;
using System.Threading.Tasks;

namespace GameSpace.Infrstructure
{
    public static class ApplicationBuilderExtensions
    {

        public static async Task<IApplicationBuilder> PrepareDataBase(
            this IApplicationBuilder app)
        {
            using var scopeServices = app.ApplicationServices.CreateScope();
            var serviceProvider = scopeServices.ServiceProvider;

            var data = serviceProvider.GetRequiredService<GameSpaceDbContext>();

            data.Database.Migrate();

            await SeedAdministrator(serviceProvider);

            return app;
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