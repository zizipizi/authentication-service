using System.Threading.Tasks;
using Authentication.Data.Repositories;
using Authentication.Host.Models;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Host.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("signout")]
        public async Task<IActionResult> SignOut()
        {
            return BadRequest("Signout unavailable");
        }

        [HttpPost("changepass")]
        public async Task<IActionResult> ChangePassword(ChangePassModel passwords)
        {
            return BadRequest("ChangePass unavailable");
        }
    }
}