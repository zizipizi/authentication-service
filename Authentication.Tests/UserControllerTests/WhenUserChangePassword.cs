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
            var tokenModel = FakeModels.FakeTokenModel();
            var changePassModel = FakeModels.FakePasswords();

            var userService = FakeUserServiceFactory.UserChangePassword(UserResult.Ok, tokenModel, "");

            var userController = new UserController(userService);
            var result = await userController.ChangePassword(changePassModel);

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
