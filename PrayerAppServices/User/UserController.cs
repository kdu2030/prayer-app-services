using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.User.Models;
using System.Net;

namespace PrayerAppServices.User {
    [ApiController]
    [Route("/api/v1/user")]
    public class UserController(IUserManager userManager) : ControllerBase, IUserController {
        private readonly IUserManager _userManager = userManager;

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserSummary))]
        public async Task<IActionResult> CreateUser(CreateUserRequest request) {
            UserSummary userSummary = await _userManager.CreateUserAsync(request);
            return Ok(userSummary);
        }

    }
}
