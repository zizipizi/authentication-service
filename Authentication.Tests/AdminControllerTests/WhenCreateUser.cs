using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Authentication.Tests.AdminControllerTests
{
    public class WhenCreateUser
    {
        [Fact]
        public async Task CreateUser_Success()
        {
            var userService = FakeAdminServiceFactory.CreateFakeUserService(AdminResult.Ok, "User created");
            var adminController = new AdminController(userService);
            var userModel = new UserCreateModel();

            var result = await adminController.CreateUser(userModel);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User created", ((OkObjectResult)result).Value);
        }

        [Fact]
        public async Task CreateUser_UserExist()
        {
            var userModel = new UserCreateModel();
            var userService = FakeAdminServiceFactory.CreateFakeUserService(AdminResult.UserExist, $"User with login {userModel.Login} exist");
            var adminController = new AdminController(userService);

            var result = await adminController.CreateUser(userModel);

            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal($"User with login {userModel.Login} exist", ((ConflictObjectResult)result).Value);
        }
    }
}
