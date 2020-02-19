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
        [ActionName("all")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _adminService.GetAllAsync();
            _logger.LogInformation("Getting all users");
            return Ok(result);
        }

        [ActionName("createuser")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(UserCreateModel model, CancellationToken cancellationToken)
        {
            var result = await _adminService.CreateUserAsync(model, cancellationToken);
            
            if (result.Value == AdminResult.Ok)
                return Ok(result.Model);

            _logger.LogWarning($"Conflict {result.Message}");
            return Conflict(result.Message);
        }

        [ActionName("deleteuser")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
        {
            var result = await _adminService.DeleteUserAsync(id, cancellationToken);

            if (result.Value == AdminResult.Ok)
                return Ok(result.Message);

            _logger.LogWarning($"{result.Message}");
            return NotFound(result.Message);
        }

        [ActionName("blockuser")]
        [HttpGet("block/{id}")]
        public async Task<IActionResult> BlockUser(int id, CancellationToken cancellationToken)
        {
            var result = await _adminService.BlockUserAsync(id, cancellationToken);

            if (result.Value == AdminResult.Ok)
                return Ok(result.Message);

            _logger.LogWarning($"{result.Message}");
            return NotFound(result.Message);
        }
    }
}