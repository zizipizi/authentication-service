using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Data.Models;
using Authentication.Host.Models;
using Microsoft.AspNetCore.Http;
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