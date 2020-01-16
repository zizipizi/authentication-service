using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Models;
using Authentication.Host.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Authentication.Tests.AdminControllerTests
{
    public class WhenCreateUser
    {
        [Fact]
        public async Task CreateUser_Success()
        {
            var userService = FakeUserServiceFactory.CreateFakeUserService(UserServiceResult.UserResult.Ok);
            var adminController = new AdminController(userService);
            var userModel = new UserCreateModel();

            var result = await adminController.CreateUser(userModel);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User created", ((OkObjectResult)result).Value);
        }

        [Fact]
        public async Task CreateUser_UserExist()
        {
            var userService = FakeUserServiceFactory.CreateFakeUserService(UserServiceResult.UserResult.Exist);
            var adminController = new AdminController(userService);
            var userModel = new UserCreateModel();

            var result = await adminController.CreateUser(userModel);

            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("User with same login exist", ((ConflictObjectResult)result).Value);
        }
    }

    public static class FakeUserServiceFactory
    {
        public static IUserService CreateFakeUserService(UserServiceResult.UserResult result)
        {
            var userServiceFake = new Mock<IUserService>();

            userServiceFake.Setup(c => c.CreateUserAsync(It.IsAny<UserCreateModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new UserServiceResult(result)));

            return userServiceFake.Object;
        }
    }
}
