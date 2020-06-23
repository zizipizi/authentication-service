using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Host.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [ActionName("refresh")]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(BodyTokenModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _authService.RefreshToken(model, cancellationToken);

            return result.ToActionResult();
        }

        [ActionName("signin")]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(LoginModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            model.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            var result = await _authService.SignIn(model, cancellationToken);

            return result.ToActionResult();
        }
    }
}