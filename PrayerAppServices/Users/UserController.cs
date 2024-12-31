using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.Users.Models;
using System.Net;

namespace PrayerAppServices.Users {
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

        [HttpPost]
        [Route("summary")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserSummary))]
        public async Task<IActionResult> GetUserSummaryFromCredentials(UserCredentials credentials) {
            UserSummary userSummary = await _userManager.GetUserSummaryFromCredentialsAsync(credentials);
            return Ok(userSummary);
        }

        [HttpGet]
        [Authorize]
        [Route("{userId}/summary")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserSummary))]
        public IActionResult GetUserSummaryFromUserId(int userId) {
            UserSummary userSummary = _userManager.GetUserSummaryFromUserId(userId);
            return Ok(userSummary);
        }

        [HttpGet]
        [Authorize]
        [Route("token")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserTokenPair))]
        public IActionResult GetUserTokenPair([FromHeader(Name = "Authorization")] string authHeader) {
            UserTokenPair tokenPair = _userManager.GetUserTokenPair(authHeader);
            return Ok(tokenPair);
        }
    }
}
