using Authentication.Host.Controllers;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Authentication.Host.Enums;
using Xunit;

namespace Authentication.Tests.AdminControllerTests
{
    public class WhenBlockUser
    {
        [Fact]
        public async Task BlockUser_Sucess()
        {
            var userService = FakeUserServiceFactory.GetFakeBlockUserService(AdminResult.Ok);
            var adminController = new AdminController(userService);

            int id = 1;

            var result = await adminController.BlockUser(id);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"User with Id {id} is blocked", ((OkObjectResult)result).Value);
        }

        [Fact]
        public async Task BlockUser_NotFound()
        {
            var userService = FakeUserServiceFactory.GetFakeBlockUserService(AdminResult.UserNotFound);
            var adminController = new AdminController(userService);
            var id = 1;

            var result = await adminController.BlockUser(id);

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User with id {id} not found", ((NotFoundObjectResult)result).Value);
        }
    }
}
