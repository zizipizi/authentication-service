using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Authentication.Tests.AdminControllerTests.Utills;
using FluentAssertions;
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
            var adminService = FakeAdminServiceFactory.CreateUser(AdminResult.Ok, $"123");
            var adminController = new AdminController(adminService, logger);
            var userModel = new UserCreateModel();

            var result = await adminController.CreateUser(userModel, CancellationToken.None);

            result.Should().BeOfType<OkObjectResult>();
            //Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateUser_UserExist()
        {
            var logger = new Mock<ILogger<AdminController>>().Object;
            var adminService = FakeAdminServiceFactory.CreateUser(AdminResult.UserExist, $"123");
            var adminController = new AdminController(adminService, logger);
            var userModel = new UserCreateModel();

            var result = await adminController.CreateUser(userModel, CancellationToken.None);

            result.Should().BeOfType<ConflictObjectResult>();
            //Assert.IsType<ConflictObjectResult>(result);
        }
    }
}
