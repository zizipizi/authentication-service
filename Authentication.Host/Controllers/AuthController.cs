using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Authentication.Host.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger = null)
        {
            _authService = authService;
            _logger = logger ?? new NullLogger<AuthController>();
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

            var result = await _authService.SignIn(model, cancellationToken);

            return result.ToActionResult();
        }
    }
}