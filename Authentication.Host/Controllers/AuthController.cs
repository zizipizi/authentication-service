using System;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models;
using Authentication.Host.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NSV.Security.JWT;
using NSV.Security.Password;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using PasswordOptions = NSV.Security.Password.PasswordOptions;

namespace Authentication.Host.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _userRepo;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public AuthController(ILogger<AuthController> logger, IUserRepository userRepo, IJwtService jwtService)
        {
            _userRepo = userRepo;
            _logger = logger;
            _jwtService = jwtService;

            _passwordService = PasswordServiceFactory.Create(new PasswordOptions());

        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var all = await _userRepo.GetAllUsersAsync(CancellationToken.None);

            return Ok(all);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(TokenModel model)
        {
            var validateResult = _jwtService.RefreshAccessToken(model.AccessToken.Value, model.RefreshToken.Value);
            if (validateResult.Result != JwtTokenResult.TokenResult.Ok)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(validateResult.Tokens);
            }
        }


        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody]SignInModel model)
        {
            var user = await _userRepo.GetUserByNameAsync(model.UserName, CancellationToken.None);

            if (user != null)
            {
                var validateResult = _passwordService.Validate(model.Password, user.Password);
                if (validateResult.Result == PasswordValidateResult.ValidateResult.Ok)
                {
                    if (user.IsActive)
                    {
                        var access = _jwtService.IssueAccessToken(user.Id.ToString(), user.Login, user.Role.Split(','));

                        return Ok(access.Tokens);
                    }
                    else
                    {
                        return Forbid("User is blocked");
                    }
                }
            }

            return NotFound("User not found");
        }

    }

}