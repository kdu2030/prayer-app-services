using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.User.Models;
using System.Net;

namespace PrayerAppServices.User {

    public interface IUserController {
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserSummary))]
        public Task<IActionResult> CreateUser(CreateUserRequest request);
    }
}
