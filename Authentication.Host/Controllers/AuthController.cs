using System;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSV.Security.JWT;

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

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(TokenModel model, CancellationToken token)
        {
            var result = await _authService.RefreshToken(model, CancellationToken.None);

            switch (result.Value)
            {
                case AuthResult.Ok:
                    return Ok(result.Value);
                case AuthResult.TokenValidationProblem:
                    return Conflict("Refresh token not validate");
                case AuthResult.TokenExpired:
                    return Unauthorized("Token expired");
            }

            return BadRequest("Error while refresh");
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
            var result = await _authService.SignIn(model, CancellationToken.None);

            switch (result.Value)
            {
                case AuthResult.Ok:
                    return Ok(result);
                case AuthResult.UserBlocked:
                    return Forbid("Bearer");
                case AuthResult.UserNotFound:
                    return NotFound("User not found");
            }

            return BadRequest("Error while signin");
        }
    }

}