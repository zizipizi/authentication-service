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
    public class WhenDeleteUser
    {
        [Fact]
        public async Task DeleteUser_Sucess()
        {
            var userService = FakeUserServiceFactory.GetFakeDeleteUserService(AdminResult.Ok);
            var adminController = new AdminController(userService);

            int id = 1;

            var result = await adminController.DeleteUser(id);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"User with id {id} deleted", ((OkObjectResult)result).Value);
        }

        [Fact]
        public async Task DeleteUser_NotFound()
        {
            var userService = FakeUserServiceFactory.GetFakeDeleteUserService(AdminResult.UserNotFound);
            var adminController = new AdminController(userService);

            int id = 1;

            var result = await adminController.DeleteUser(id);

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found", ((NotFoundObjectResult)result).Value);
        }
    }
}
