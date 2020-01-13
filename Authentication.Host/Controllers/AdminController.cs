using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Interfaces;
using Authentication.Data.Models;
using Authentication.Host.Models;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Host.Controllers
{
    [Route("api/superuser")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepo;

        public AdminController(IUserService userService, IUserRepository userRepo)
        {
            _userService = userService;
            _userRepo = userRepo;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(UserCreateModel model)
        {
            var user = await _userRepo.GetUserByNameAsync(model.Login, CancellationToken.None);

            if (user != null)
            {
                return Conflict($"Login {model.Login} is already used");
            }

            await _userService.CreateUserAsync(model, CancellationToken.None);

            return Ok($"user {model.Login} created");
        }

        [HttpDelete("delete/{id}")]
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