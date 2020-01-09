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
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _userRepo;
        private readonly IPasswordService _passwordService;

        public AuthController(ILogger<AuthController> logger, IUserRepository userRepo)
        {
            _userRepo = userRepo;
            _logger = logger;


            _passwordService = PasswordServiceFactory.Create(new PasswordOptions());
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var all = await _userRepo.GetAllUsersAsync(CancellationToken.None);

            return Ok(all);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser([FromBody]UserEntity model)
        {
            if (await _userRepo.GetUserByName(model.Login, CancellationToken.None) != null)
            {
                return Conflict($"Login {model.Login} is already used");
            }
            else
            {
                var pass = _passwordService.Hash(model.Password);

                var user = await _userRepo.CreateUser(new UserEntity()
                {
                    Login = model.Login,
                    Password = pass.Hash,
                    Role = model.Role
                }, CancellationToken.None);

                return Ok($"User {model.Login} created");
            }
        }


        [HttpPost("Signin")]
        public async Task<IActionResult> SignIn([FromBody]CredentialModel model)
        {
            var user = await _userRepo.GetUserByName(model.UserName, CancellationToken.None);
            if (user != null)
            {
                var validateResult = _passwordService.Validate(model.Password, user.Password);
                if (validateResult.Result == PasswordValidateResult.ValidateResult.Ok)
                {
                    if (user.IsActive)
                    {
                        var jwtService = JwtServiceFactory.Create(TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10));
                        var access = jwtService.IssueAccessToken(user.Id.ToString(), user.Login, user.Role.Split(','));

                        return Ok(access.Tokens);
                    }
                    else
                    {
                        return NotFound("User is blocked");
                    }
                }
            }

            return BadRequest("User not found");
        }

    }

}