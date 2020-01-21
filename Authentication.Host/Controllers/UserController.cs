using System.Threading.Tasks;
using Authentication.Data.Repositories;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Mvc;
using NSV.Security.JWT;

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
        public async Task<IActionResult> SignOut(TokenModel model)
        {
            var result = await _userService.SignOut(model);

            switch (result.Value)
            {
                case UserResult.RefreshTokenExpired:
                    return Unauthorized(result.Message);
                case UserResult.RefreshNotMatchAccess:
                    return Conflict(result.Message);
            }

            return NoContent();
        }

        [HttpPost("changepass")]
        public async Task<IActionResult> ChangePassword(ChangePassModel passwords)
        {
            var result = await _userService.ChangePassword(passwords);

            switch (result.Value)
            {
                case UserResult.WrongPassword:
                    return BadRequest(result.Message);
                case UserResult.PasswordChangedNeedAuth:
                    return NoContent();
                case UserResult.Ok:
                    return Ok(result.Model);
            }

            return NotFound(result.Message);
        }
    }
}