using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Repositories;
using Authentication.Host.Models;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSV.Security.JWT;
using NSV.Security.Password;

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
        private readonly IUserService _userService;

        public AuthController(ILogger<AuthController> logger, IUserRepository userRepo, IJwtService jwtService, IPasswordService passwordService, IUserService userService)
        {
            _userRepo = userRepo;
            _logger = logger;
            _jwtService = jwtService;
            _passwordService = passwordService;
            _userService = userService;
        }
        [Authorize]
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
            return Ok(validateResult.Tokens);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
            var result = await _userService.SignIn(model);

            switch (result.Result)
            {
                case UserServiceResult.UserResult.Blocked:
                    return Forbid("Bearer");
                case UserServiceResult.UserResult.Ok:
                    return Ok(result.Token);
            }

            return NotFound("User not found");
        }

        //[HttpPost("signin")]
        //public async Task<IActionResult> SignIn([FromBody]LoginModel model)
        //{
        //    var user = await _userRepo.GetUserByNameAsync(model.UserName, CancellationToken.None);

        //    if (user != null)
        //    {
        //        var validateResult = _passwordService.Validate(model.Password, user.Password);
        //        if (validateResult.Result == PasswordValidateResult.ValidateResult.Ok)
        //        {
        //            if (user.IsActive)
        //            {
        //                var access = _jwtService.IssueAccessToken(user.Id.ToString(), user.Login, user.Role.Split(','));

        //                return Ok(access.Tokens);
        //            }
        //            return Forbid("User is blocked");
        //        }
        //    }
        //    return NotFound("User not found");
        //}

    }

}