using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Enums;
using Authentication.Host.Models;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Host.Controllers
{
    [Route("api/superuser")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(UserCreateModel model)
        {
            var result = await _adminService.CreateUserAsync(model, CancellationToken.None);

            if (result.Value == AdminResult.UserExist)
                return Conflict($"User with login {model.Login} exist");

            return Ok("User created");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _adminService.DeleteUserAsync(id, CancellationToken.None);

            if (result.Value == AdminResult.Ok)
            {
                return Ok($"User with id {id} deleted");
            }
            return NotFound("User not found");
        }

        [HttpGet("block/{id}")]
        public async Task<IActionResult> BlockUser(int id)
        {
            var result = await _adminService.BlockUserAsync(id, CancellationToken.None);

            if (result.Value == AdminResult.Ok)
            {
                return Ok($"User with Id {id} is blocked");
            }
            return NotFound($"User with id {id} not found");
        }
    }
}