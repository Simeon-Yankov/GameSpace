using AutoMapper;
using GameSpace.Infrstructure;
using GameSpace.Models.User;
using GameSpace.Services.Users.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameSpace.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService users;
        private readonly IMapper mapper;

        public UserController(IUserService users, IMapper mapper)
        {
            this.users = users;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult Profile(string userId = null)
        {
            userId ??= this.User.Id();

            var profileData = this.users.Profile(userId);

            return View(profileData);
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            var profileData = this.users.Profile(id);

            var profileForm = this.mapper.Map<EditUserFormModel>(profileData);

            return View(profileForm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditUserFormModel profile)
        {
            if (!this.users.ExcistsById(profile.Id))
            {
                return BadRequest();
            }

            if (this.users.ExcistsWantedName(this.users.GetNickname(profile.Id), profile.Nickname))
            {
                this.ModelState.AddModelError(nameof(profile.Nickname), "There is already a user with this given name.");
            }

            if (!this.ModelState.IsValid)
            {
                return View(profile);
            }

           await this.users.Edit(
                profile.Id,
                profile.Nickname,
                profile.Biography,
                profile?.Appearance?.Image,
                profile?.Appearance?.Banner,
                profile.SocialNetwork.YoutubeUrl,
                profile.SocialNetwork.TwitchUrl,
                profile.SocialNetwork.TwitterUrl,
                profile.SocialNetwork.FacebookUrl);

            return RedirectToAction(nameof(UserController.Profile), "User");
        }
    }
}