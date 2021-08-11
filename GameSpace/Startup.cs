using System.Threading.Tasks;

using GameSpace.Data;
using GameSpace.Data.Models;
using GameSpace.Infrstructure;
using GameSpace.Services.Appearances;
using GameSpace.Services.Appearances.Contracts;
using GameSpace.Services.Messages;
using GameSpace.Services.Messages.Contracts;
using GameSpace.Services.Regions;
using GameSpace.Services.Regions.Contracts;
using GameSpace.Services.Sumonners;
using GameSpace.Services.Sumonners.Contracts;
using GameSpace.Services.Teams;
using GameSpace.Services.Teams.Contracts;
using GameSpace.Services.Users;
using GameSpace.Services.Users.Contracts;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GameSpace
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<GameSpaceDbContext>(options => options
                .UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDefaultIdentity<User>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<GameSpaceDbContext>();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });

            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<IAppearanceService, AppearanceService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<ISummonerService, SummonerService>();
            services.AddTransient<IRegionService, RegionService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Task
                .Run(async () =>
                {
                    await app.PrepareDataBase();
                })
                .GetAwaiter()
                .GetResult();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}