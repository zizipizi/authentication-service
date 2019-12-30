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
        public IActionResult CreateUser([FromBody]UserEntity model)
        {
            var pass = _passwordService.Hash(model.Password);
            var user = _userRepo.CreateUser(new UserEntity()
            {
                Login = model.Login,
                Password = pass.Hash,
                Created = DateTime.Now,
                IsActive = true,
                Role = model.Role
            });

            
            return Ok($"User {user.Login} created" );
        }


        [HttpPost("signin")]
        public IActionResult SignIn([FromBody]CredentialModel model)
        {
            var user = _userRepo.GetUserByName(model.UserName);
            if (user != null)
            {
                var validateResult = _passwordService.Validate(model.Password, user.Password);
                if (validateResult.Result == PasswordValidateResult.ValidateResult.Ok)
                {
                    var jwtService = JwtServiceFactory.Create(TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(5),
                        TimeSpan.FromSeconds(10));
                    var access = jwtService.IssueAccessToken(user.Id.ToString(), user.Login, user.Role.Split(','));


                    return Ok(access.Tokens);
                }
            }

            return BadRequest("User not found");
        }

    }

}