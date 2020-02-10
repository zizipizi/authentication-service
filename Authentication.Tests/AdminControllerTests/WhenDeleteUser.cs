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
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Authentication.Tests.AdminControllerTests
{
    public class WhenDeleteUser
    {
        [Fact]
        public async Task DeleteUser_Sucess()
        {
            int id = 1;
            var logger = new Mock<ILogger<AdminController>>().Object;
            var userService = FakeAdminServiceFactory.GetFakeDeleteUserService(AdminResult.Ok, $"User with id {id} deleted");
            var adminController = new AdminController(userService, logger);

            var result = await adminController.DeleteUser(id, CancellationToken.None);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"User with id {id} deleted", ((OkObjectResult)result).Value);
        }

        [Fact]
        public async Task DeleteUser_NotFound()
        {
            int id = 1;
            var logger = new Mock<ILogger<AdminController>>().Object;
            var userService = FakeAdminServiceFactory.GetFakeDeleteUserService(AdminResult.UserNotFound, $"User with {id} not found");
            var adminController = new AdminController(userService, logger);

            var result = await adminController.DeleteUser(id, CancellationToken.None);

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User with {id} not found", ((NotFoundObjectResult)result).Value);
        }

    }
}
