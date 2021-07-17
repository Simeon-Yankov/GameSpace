using GameSpace.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameSpace.Infrstructure
{
    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder PrepareDataBase(
            this IApplicationBuilder app)
        {
            using var scopeServices = app.ApplicationServices.CreateScope();

            var data = scopeServices.ServiceProvider.GetService<GameSpaceDbContext>();

            data.Database.Migrate();

            return app;
        }
    }
}