using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Authentication.Host.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/superuser")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [ActionName("all")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _adminService.GetAllUsersAsync(cancellationToken);

            return result.ToActionResult();
        }

        [ActionName("createuser")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(UserCreateModel model, CancellationToken cancellationToken)
        {
            var result = await _adminService.CreateUserAsync(model, cancellationToken);

            return result.ToActionResult();
        }

        [ActionName("deleteuser")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
        {
            var result = await _adminService.DeleteUserAsync(id, cancellationToken);

            return result.ToActionResult();
        }

        [ActionName("blockuser")]
        [HttpGet("block/{id}")]
        public async Task<IActionResult> BlockUser(long id, CancellationToken cancellationToken)
        {
            var result = await _adminService.BlockUserAsync(id, cancellationToken);

            return result.ToActionResult();
        }
    }
}