using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSV.Security.JWT;

namespace Authentication.Host.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
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

            _logger.LogWarning("No content");
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

            _logger.LogInformation($"{result.Message}");
            return NotFound(result.Message);
        }
    }
}