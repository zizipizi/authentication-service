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
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using FluentAssertions;

namespace Authentication.Tests.AdminControllerTests
{
    public class WhenBlockUser
    {
        [Fact]
        public async Task BlockUser_Sucess()
        {
            int id = 1;
            var logger = new Mock<ILogger<AdminController>>().Object;
            var adminService = FakeAdminServiceFactory.BlockUser(AdminResult.Ok, $"User with Id {id} is blocked");
            var adminController = new AdminController(adminService, logger);

            var result = await adminController.BlockUser(id, CancellationToken.None);

            result.Should().BeOfType<OkObjectResult>();
            ((OkObjectResult) result).Value.Should().BeEquivalentTo($"User with Id {id} is blocked");

            //Assert.IsType<OkObjectResult>(result);
            //Assert.Equal($"User with Id {id} is blocked", ((OkObjectResult)result).Value);
        }

        [Fact]
        public async Task BlockUser_NotFound()
        {
            int id = 1;
            var logger = new Mock<ILogger<AdminController>>().Object;
            var adminService = FakeAdminServiceFactory.BlockUser(AdminResult.UserNotFound, $"User with id {id} not found");
            var adminController = new AdminController(adminService, logger);

            var result = await adminController.BlockUser(id, CancellationToken.None);

            result.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)result).Value.Should().BeEquivalentTo($"User with id {id} not found");

            //Assert.IsType<NotFoundObjectResult>(result);
            //Assert.Equal($"User with id {id} not found", ((NotFoundObjectResult)result).Value);
        }
    }
}
