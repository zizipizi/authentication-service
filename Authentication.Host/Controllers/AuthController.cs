﻿using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Authentication.Host.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(BodyTokenModel model, CancellationToken cancellationToken)
        {
            var result = await _authService.RefreshToken(model, cancellationToken);

            switch (result.Value)
            {
                case AuthResult.Ok:
                    return Ok(result.Model);
                case AuthResult.TokenValidationProblem:
                    return Conflict(result.Message);
                case AuthResult.TokenIsBlocked:
                    return Unauthorized(result.Message);
            }

            return BadRequest("Error while refresh");
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(LoginModel model, CancellationToken cancellationToken)
        {
            var result = await _authService.SignIn(model, cancellationToken);

            switch (result.Value)
            {
                case AuthResult.UserNotFound:
                    return NotFound(result.Message);
                case AuthResult.Ok:
                    return Ok(result.Model);
                case AuthResult.UserBlocked:
                    return Unauthorized(result.Message);
            }

            _logger.LogError(result.Message);
            return BadRequest(result.Message);
        }
    }

}