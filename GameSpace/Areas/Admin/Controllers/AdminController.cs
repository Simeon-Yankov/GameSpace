using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static GameSpace.Areas.Admin.AdminConstants;

namespace GameSpace.Areas.Admin.Controllers
{

    [Area(AreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public abstract class AdminController : Controller
    {
    }
}