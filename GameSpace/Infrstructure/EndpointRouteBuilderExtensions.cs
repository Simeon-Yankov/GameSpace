using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace GameSpace.Infrstructure
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void MapDefaultAreaRoute(this IEndpointRouteBuilder endpoints)
            => endpoints.MapControllerRoute(
                "Areas",
                "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    }
}