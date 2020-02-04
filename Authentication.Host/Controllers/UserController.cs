using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using NSV.Security.JWT;

namespace Authentication.Host.Controllers
{
    [Authorize]
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

        [HttpPost("signout")]
        public async Task<IActionResult> SignOut(BodyTokenModel model)
        {
            var token = "";

            var id = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).ToList()[1].Value;

            var authHeader = Request.Headers["Authorization"].ToString();

            if (authHeader != null && authHeader.Contains("Bearer"))
            {
                token = authHeader.Replace("Bearer", "");
            }

            var result = await _userService.SignOut(model, id, token, CancellationToken.None);

            if (result.Value == UserResult.Ok)
            {
                return NoContent();
            }

            return Ok(result.Message);
        }


        [HttpPost("changepass")]
        public async Task<IActionResult> ChangePassword(ChangePassModel passwords)
        {
            var id = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).ToList()[1].Value;
            var token = "";
            var authHeader = Request.Headers["Authorization"].ToString();

            if (authHeader != null && authHeader.Contains("Bearer"))
            { 
                token = authHeader.Replace("Bearer", "");
            }

            var result = await _userService.ChangePasswordAsync(passwords, id, token, CancellationToken.None);

            switch (result.Value)
            {
                case UserResult.WrongPassword:
                    return BadRequest(result.Message);
                case UserResult.PasswordChangedNeedAuth:
                    return NoContent();
                case UserResult.Ok:
                    return Ok(result.Model);
                case UserResult.Error:
                    return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            _logger.LogInformation($"{result.Message}");
            return NotFound(result.Message);
        }
    }
}