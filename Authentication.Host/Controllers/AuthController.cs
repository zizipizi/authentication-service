using System;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Repositories;
using Authentication.Host.Enums;
using Authentication.Host.Models;
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
            throw new Exception();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
            var result = await _authService.SignIn(model, CancellationToken.None);

            switch (result.Value)
            {
                case AuthResult.UserBlocked:
                    return Forbid("Bearer");
                case AuthResult.Ok:
                    return Ok(result);
            }

            return NotFound("User not found");
        }
    }

}