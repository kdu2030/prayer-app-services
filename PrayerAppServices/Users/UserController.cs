using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.Users.Models;
using System.Net;

namespace PrayerAppServices.Users
{
    [ApiController]
    [Route("/api/user")]
    public class UserController(IUserManager userManager) : ControllerBase, IUserController
    {
        private readonly IUserManager _userManager = userManager;

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserSummary))]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            UserSummary userSummary = await _userManager.CreateUserAsync(request);
            return Ok(userSummary);
        }

        /// <summary>
        /// Logs in a user and fetches user details
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("summary")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserSummary))]
        public async Task<IActionResult> GetUserSummaryFromCredentials(UserCredentials credentials)
        {
            UserSummary userSummary = await _userManager.GetUserSummaryFromCredentialsAsync(credentials);
            return Ok(userSummary);
        }

        /// <summary>
        /// Gets user details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("{userId}/summary")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserSummary))]
        public async Task<IActionResult> GetUserSummaryFromUserIdAsync(int userId)
        {
            UserSummary userSummary = await _userManager.GetUserSummaryFromUserIdAsync(userId);
            return Ok(userSummary);
        }

        /// <summary>
        /// Get user's access token and refresh token
        /// </summary>
        /// <param name="authHeader"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("token")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserTokenPair))]
        public IActionResult GetUserTokenPair([FromHeader(Name = "Authorization")] string authHeader)
        {
            UserTokenPair tokenPair = _userManager.GetUserTokenPair(authHeader);
            return Ok(tokenPair);
        }
    }
}
