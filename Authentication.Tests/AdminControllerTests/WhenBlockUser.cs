using Authentication.Host.Controllers;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Authentication.Host.Results.Enums;
using Authentication.Tests.AdminControllerTests.Utills;
using Xunit;

namespace Authentication.Tests.AdminControllerTests
{
    public class WhenBlockUser
    {
        [Fact]
        public async Task BlockUser_Sucess()
        {
            int id = 1;
            var userService = FakeAdminServiceFactory.GetFakeBlockUserService(AdminResult.Ok, $"User with Id {id} is blocked");
            var adminController = new AdminController(userService);

            var result = await adminController.BlockUser(id);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"User with Id {id} is blocked", ((OkObjectResult)result).Value);
        }

        [Fact]
        public async Task BlockUser_NotFound()
        {
            int id = 1;
            var userService = FakeAdminServiceFactory.GetFakeBlockUserService(AdminResult.UserNotFound, $"User with id {id} not found");
            var adminController = new AdminController(userService);

            var result = await adminController.BlockUser(id);

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User with id {id} not found", ((NotFoundObjectResult)result).Value);
        }
    }
}
