using System.Threading.Tasks;

using GameSpace.Controllers;
using GameSpace.Services.HttpClients.Contracts;
using GameSpace.Services.HttpClients.Models;

using Microsoft.AspNetCore.Mvc;

namespace GameSpace.Areas.Admin.Controllers
{
    public class APIController : AdminController
    {
        private const string RiotAPI = "RiotAPI";

        private readonly IClientService client;

        public APIController(IClientService client)
        {
            this.client = client;
        }

        public IActionResult Update() => View();

        [HttpPost]
        public async Task<IActionResult> Update(APIServiceModel api)
        {
            if (!this.ModelState.IsValid)
            {
                return View(api);
            }

            await this.client.UpdateAPIAsync(RiotAPI, api.Value);

            return RedirectToAction(nameof(HomeController.Index), "Home", new { area = ""});
        }
    }
}