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
            var result = await CreateUser(AdminResult.Ok, "User created");

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User created", ((OkObjectResult)result).Value);
        }

        [Fact]
        public async Task CreateUser_UserExist()
        {

            var result = await CreateUser(AdminResult.UserExist, $"User with same login exist");

            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal($"User with same login exist", ((ConflictObjectResult)result).Value);
        }

        public async Task<IActionResult> CreateUser(AdminResult expectationResult, string message = "")
        {
            var userService = FakeAdminServiceFactory.CreateFakeUserService(expectationResult, message);
            var adminController = new AdminController(userService);
            var userModel = new UserCreateModel();

            var result = await adminController.CreateUser(userModel);

            return result;
        }
    }
}
