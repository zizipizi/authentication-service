using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
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

            if (result.Value == AdminResult.Ok)
                return Ok(result.Message);

            return Conflict(result.Message);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _adminService.DeleteUserAsync(id, CancellationToken.None);

            if (result.Value == AdminResult.Ok)
            {
                return Ok(result.Message);
            }
            return NotFound(result.Message);
        }

        [HttpGet("block/{id}")]
        public async Task<IActionResult> BlockUser(int id)
        {
            var result = await _adminService.BlockUserAsync(id, CancellationToken.None);

            if (result.Value == AdminResult.Ok)
            {
                return Ok(result.Message);
            }
            return NotFound(result.Message);
        }
    }
}