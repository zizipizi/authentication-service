using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models;
using Authentication.Host.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSV.Security.Password;

namespace Authentication.Host.Controllers
{
    [Route("api/superuser")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IUserRepository _userRepo;
        private IPasswordService _passwordService;
        public AdminController(IUserRepository userRepository)
        {
            _userRepo = userRepository;
            _passwordService = PasswordServiceFactory.Create(new PasswordOptions());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateModel user)
        {
            if (await _userRepo.GetUserByNameAsync(user.Login, CancellationToken.None) != null)
            {
                return Conflict($"Login {user.Login} is already used");
            }
            else
            {
                var pass = _passwordService.Hash(user.Password);

                var newUser = await _userRepo.CreateUserAsync(new UserEntity()
                {
                    Login = user.Login,
                    Password = pass.Hash,
                    Role = "Admin"
                }, CancellationToken.None);

                return Ok($"User {user.Login} created");
            }
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            return BadRequest("Delete unavailable");
        }

        [HttpGet("block/{id}")]
        public async Task<IActionResult> BlockUser(int id)
        {
            return BadRequest("Blocking unavailable");
        }
    }
}