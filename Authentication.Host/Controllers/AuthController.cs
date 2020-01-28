﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSV.Security.JWT;

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
        public async Task<IActionResult> RefreshToken(TokenModel model, CancellationToken token)
        {
            var result = await _authService.RefreshToken(model, CancellationToken.None);

            switch (result.Value)
            {
                case AuthResult.Ok:
                    return Ok(result.Value);
                case AuthResult.TokenValidationProblem:
                    _logger.LogWarning($"{result.Message}");
                    return Conflict("Refresh token not validate");
                case AuthResult.TokenExpired:
                    _logger.LogWarning($"{result.Message}");
                    return Unauthorized("Token expired");
            }
            _logger.LogWarning("Error while refresh");
            return BadRequest("Error while refresh");
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
            var result = await _authService.SignIn(model, CancellationToken.None);

            if (result.Value == AuthResult.UserNotFound)
            {
                _logger.LogWarning($"{result.Message}");
                return NotFound(result.Message);
            }

            switch (result.Value)
            {
                case AuthResult.Ok:
                    return Ok(result.Model);
                case AuthResult.UserBlocked:
                    return Forbid("Bearer");
                case AuthResult.UserNotFound:
                    return NotFound(result.Message);
            }

            _logger.LogWarning($"{result.Message}");
            return BadRequest(result.Message);
        }
    }

}