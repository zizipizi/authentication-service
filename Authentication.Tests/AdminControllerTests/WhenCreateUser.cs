using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Authentication.Tests.AdminControllerTests.Utills;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Authentication.Tests.AdminControllerTests
{
    public class WhenCreateUser
    {
        [Fact]
        public async Task CreateUser_Success()
        {
            var logger = new Mock<ILogger<AdminController>>().Object;
            var userService = FakeAdminServiceFactory.CreateFakeUserService(AdminResult.Ok, $"123");
            var adminController = new AdminController(userService, logger);
            var userModel = new UserCreateModel();

            var result = await adminController.CreateUser(userModel);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateUser_UserExist()
        {
            var logger = new Mock<ILogger<AdminController>>().Object;
            var userService = FakeAdminServiceFactory.CreateFakeUserService(AdminResult.UserExist, $"123");
            var adminController = new AdminController(userService, logger);
            var userModel = new UserCreateModel();

            var result = await adminController.CreateUser(userModel);

            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public async Task CreateUser_GetMessageEqualAdminServiceResult()
        {
            var userService = FakeAdminServiceFactory.CreateFakeUserService(AdminResult.Ok, $"Simple text");
            var logger = new Mock<ILogger<AdminController>>().Object;

            var adminController = new AdminController(userService, logger);
            var userModel = new UserCreateModel();

            var result = await adminController.CreateUser(userModel);

            Assert.Equal("Simple text", ((ObjectResult)result).Value);
        }
    }
}
