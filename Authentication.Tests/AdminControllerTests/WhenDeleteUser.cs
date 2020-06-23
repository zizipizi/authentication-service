using Authentication.Host.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Tests.AdminControllerTests.Utills;
using FluentAssertions;
using Xunit;

namespace Authentication.Tests.AdminControllerTests
{
    public class WhenDeleteUser
    {
        [Fact]
        public async Task DeleteUser_Success()
        {
            int id = 1;
            var adminService = FakeAdminServiceFactory.DeleteUser(HttpStatusCode.OK);
            var adminController = new AdminController(adminService);

            var result = await adminController.DeleteUser(id, CancellationToken.None);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task DeleteUser_NotFound()
        {
            int id = 1;
            var adminService = FakeAdminServiceFactory.DeleteUser(HttpStatusCode.NotFound);
            var adminController = new AdminController(adminService);

            var result = await adminController.DeleteUser(id, CancellationToken.None);

            result.Should().BeOfType<NotFoundResult>();
        }

    }
}
