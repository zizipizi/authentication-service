using System.Threading.Tasks;
using Authentication.Data.Repositories;
using Authentication.Host.Models;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Host.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("signout")]
        public async Task<IActionResult> SignOut()
        {
            return BadRequest("Signout unavailable");
        }

        [HttpPost("changepass")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassModel passwords)
        {
            return BadRequest("ChangePass unavailable");
        }


    }
}