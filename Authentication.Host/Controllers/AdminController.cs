using System.Threading;
using System.Threading.Tasks;
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

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(UserCreateModel model)
        {
            var result = await _userService.CreateUserAsync(model, CancellationToken.None);

            if (result.Result == UserServiceResult.UserResult.Exist)
                return Conflict("User with same login exist");
            return Ok("User created");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id, CancellationToken.None);
            if (result.Result == UserServiceResult.UserResult.Ok)
            {
                return Ok($"User with id {id} deleted");
            }
            return NotFound("User not found");
        }

        [HttpGet("block/{id}")]
        public async Task<IActionResult> BlockUser(int id)
        {
            var result = await _userService.BlockUserAsync(id, CancellationToken.None);

            if (result.Result == UserServiceResult.UserResult.Ok)
            {
                return Ok($"User with Id {id} is blocked");
            }

            return NotFound("User not found");
        }
    }
}