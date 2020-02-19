using Authentication.Host.Controllers;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Results.Enums;
using Authentication.Tests.AdminControllerTests.Utills;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Authentication.Tests.AdminControllerTests
{
    public class WhenDeleteUser
    {
        [Fact]
        public async Task DeleteUser_Success()
        {
            int id = 1;
            var logger = new Mock<ILogger<AdminController>>().Object;
            var adminService = FakeAdminServiceFactory.DeleteUser(AdminResult.Ok, $"User with id {id} deleted");
            var adminController = new AdminController(adminService, logger);

            var result = await adminController.DeleteUser(id, CancellationToken.None);

            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult) result).Value.Should().BeEquivalentTo($"User with id {id} deleted");

            //Assert.IsType<OkObjectResult>(result);
            //Assert.Equal($"User with id {id} deleted", ((OkObjectResult)result).Value);
        }

        [Fact]
        public async Task DeleteUser_NotFound()
        {
            int id = 1;
            var logger = new Mock<ILogger<AdminController>>().Object;
            var adminService = FakeAdminServiceFactory.DeleteUser(AdminResult.UserNotFound, $"User with {id} not found");
            var adminController = new AdminController(adminService, logger);

            var result = await adminController.DeleteUser(id, CancellationToken.None);

            result.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult) result).Value.Should().BeEquivalentTo($"User with {id} not found");

            //Assert.IsType<NotFoundObjectResult>(result);
            //Assert.Equal($"User with {id} not found", ((NotFoundObjectResult)result).Value);
        }

    }
}
