using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Microsoft.AspNetCore.Mvc;
using NSV.Security.JWT;
using Xunit;

namespace Authentication.Tests.UserControllerTests
{
    public class WhenUserChangePassword
    {
        [Fact]
        public async Task UserChangePassword_Ok()
        {
            var result = await UserChangePassword(UserResult.Ok);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UserChangePassword_WrongPassword()
        {
            var result = await UserChangePassword(UserResult.WrongPassword);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UserChangePassword_NeedAuth()
        {
            var result = await UserChangePassword(UserResult.PasswordChangedNeedAuth);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UserChangePassword_NotFound()
        {
            var result = await UserChangePassword(UserResult.UserNotFound);

            Assert.IsType<NotFoundObjectResult>(result);
        }


        public async Task<IActionResult> UserChangePassword(UserResult expectationResult, string message = "")
        {
            var tokenModel = FakeModels.FakeTokenModel();
            var changePassModel = FakeModels.FakePasswords();

            var userService = FakeUserServiceFactory.UserChangePassword(expectationResult, tokenModel, "");

            var userController = new UserController(userService);
            var result = await userController.ChangePassword(changePassModel);

            return result;
        }
    }
}
