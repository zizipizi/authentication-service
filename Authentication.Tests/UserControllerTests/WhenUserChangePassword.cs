using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Tests.UserControllerTests.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NSV.Security.JWT;
using Xunit;

namespace Authentication.Tests.UserControllerTests
{
    public class WhenUserChangePassword
    {
        [Fact]
        public async Task UserChangePassword_Ok()
        {
            var tokenModel = FakeModels.FakeTokenModel();
            var changePassModel = FakeModels.FakePasswords();
            
            var logger = new Mock<ILogger<UserController>>().Object;

            var userService = FakeUserServiceFactory.UserChangePassword(UserResult.Ok, tokenModel, "");

            var userController = new UserController(userService, logger);
            var result = await userController.ChangePassword(changePassModel);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UserChangePassword_WrongPassword()
        {
            var tokenModel = FakeModels.FakeTokenModel();
            var changePassModel = FakeModels.FakePasswords();

            var logger = new Mock<ILogger<UserController>>().Object;


            var userService = FakeUserServiceFactory.UserChangePassword(UserResult.WrongPassword, tokenModel, "");

            var userController = new UserController(userService, logger);
            var result = await userController.ChangePassword(changePassModel);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UserChangePassword_NeedAuth()
        {
            var tokenModel = FakeModels.FakeTokenModel();
            var changePassModel = FakeModels.FakePasswords();

            var logger = new Mock<ILogger<UserController>>().Object;

            var userService = FakeUserServiceFactory.UserChangePassword(UserResult.PasswordChangedNeedAuth, tokenModel, "");

            var userController = new UserController(userService, logger);
            var result = await userController.ChangePassword(changePassModel);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UserChangePassword_NotFound()
        {
            var tokenModel = FakeModels.FakeTokenModel();
            var changePassModel = FakeModels.FakePasswords();

            var logger = new Mock<ILogger<UserController>>().Object;

            var userService = FakeUserServiceFactory.UserChangePassword(UserResult.UserNotFound, tokenModel, "");

            var userController = new UserController(userService, logger);
            var result = await userController.ChangePassword(changePassModel);

            Assert.IsType<NotFoundObjectResult>(result);
        }

    }
}
