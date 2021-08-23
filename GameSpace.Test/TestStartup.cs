using GameSpace.Services.Messages.Contracts;
using GameSpace.Services.Users.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MyTested.AspNetCore.Mvc;

namespace GameSpace.Test
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) 
            : base(configuration)
        {
        }

        public void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.ReplaceDbContext();
            services.ReplaceTransient<IMessageService>(_ => Mocks.MessageServiceMock.Instance);
            services.ReplaceTransient<IUserService>(_ => Mocks.UserServiceMock.Instance);
        }
    }
}