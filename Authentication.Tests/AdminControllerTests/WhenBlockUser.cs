using Authentication.Host.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Tests.AdminControllerTests.Utills;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace Authentication.Tests.AdminControllerTests
{
    public class WhenBlockUser
    {
        [Fact]
        public async Task BlockUser_Sucess()
        {
            long id = 1;
            var adminService = FakeAdminServiceFactory.BlockUser(HttpStatusCode.OK);
            var adminController = new AdminController(adminService);

            var result = await adminController.BlockUser(id, CancellationToken.None);

            result.Should().BeOfType<OkResult>();
            //((OkObjectResult) result).Value.Should().BeEquivalentTo($"User with Id {id} is blocked");
        }

        [Fact]
        public async Task BlockUser_NotFound()
        {
            long id = 1;
            var adminService = FakeAdminServiceFactory.BlockUser(HttpStatusCode.NotFound);
            var adminController = new AdminController(adminService);

            var result = await adminController.BlockUser(id, CancellationToken.None);

            result.Should().BeOfType<NotFoundResult>();
            //((NotFoundObjectResult)result).Value.Should().BeEquivalentTo($"User with id {id} not found");
        }

        [Fact]
        public async Task BlockUser_ServiceUnavailable()
        {
            long id = 1;
            var adminService = FakeAdminServiceFactory.BlockUser(HttpStatusCode.ServiceUnavailable);
            var adminController = new AdminController(adminService);

            var result = await adminController.BlockUser(id, CancellationToken.None);

            result.Should().Match<StatusCodeResult>(c => c.StatusCode == StatusCodes.Status503ServiceUnavailable);
        }
    }
}
