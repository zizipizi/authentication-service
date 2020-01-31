using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Authentication.Host.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/superuser")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger _logger;

        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = _adminService.GetAll();
            _logger.LogInformation("Getting all users");
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(UserCreateModel model)
        {
            var result = await _adminService.CreateUserAsync(model, CancellationToken.None);

            if (result.Value == AdminResult.Ok)
                return Ok(result.Message);

            _logger.LogWarning($"Conflict {result.Message}");
            return Conflict(result.Message);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _adminService.DeleteUserAsync(id, CancellationToken.None);

            if (result.Value == AdminResult.Ok)
                return Ok(result.Message);

            _logger.LogWarning($"{result.Message}");
            return NotFound(result.Message);
        }

        [HttpGet("block/{id}")]
        public async Task<IActionResult> BlockUser(int id)
        {
            var result = await _adminService.BlockUserAsync(id, CancellationToken.None);

            if (result.Value == AdminResult.Ok)
                return Ok(result.Message);

            _logger.LogWarning($"{result.Message}");
            return NotFound(result.Message);
        }
    }
}