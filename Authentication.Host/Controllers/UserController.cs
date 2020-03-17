using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Authentication.Host.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [ActionName("signout")]
        [HttpPost("signout")]
        public async Task<IActionResult> SignOut(BodyTokenModel model, CancellationToken cancellationToken)
        {
            var idStr = GetUserInfo().id;

            if (!long.TryParse(idStr, out var id))
            {
                return BadRequest("Wrong identifier");
            }

            var refreshJti = GetJti(model.RefreshToken);

            var result = await _userService.SignOutAsync(id, refreshJti, cancellationToken);

            return result.ToActionResult();
        }

        [ActionName("changepass")]
        [HttpPost("changepass")]
        public async Task<IActionResult> ChangePassword(ChangePassModel passwords, CancellationToken cancellationToken)
        {
            var (idStr, token) = GetUserInfo();
            if (!long.TryParse(idStr, out var id))
            {
                return BadRequest("Wrong identifier");
            }
            var result = await _userService.ChangePasswordAsync(passwords, id, token, cancellationToken);

            return result.ToActionResult();
        }

        private (string id, string token) GetUserInfo()
        {
            var userId = GetIdentifier();
            var userToken = "";
            var authHeader = Request.Headers["Authorization"].ToString();

            if (authHeader != null && authHeader.Contains("Bearer"))
            {
                userToken = authHeader.Replace("Bearer", "");
            }

            return (userId, userToken);
        }

        private string GetIdentifier()
        {
            return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        private string GetJti(string jwtToken)
        {
            return new JwtSecurityToken(jwtToken).Claims.FirstOrDefault(c => c.Type == "jti")?.Value;
        }
    }
}