using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Results.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NSV.Security.JWT;
using Xunit;

namespace Authentication.Tests.UserControllerTests
{
    public class WhenUserSignOut
    {
        [Fact]
        public async Task SignOut_TokenExpired()
        {
            var userService = FakeUserServiceFactory.UserSignOut(UserResult.RefreshTokenExpired, "Refresh token expired");
            var userController = new UserController(userService);

            var tokenModel = new TokenModel(("asd", DateTime.Now),("sd", DateTime.Now));

            var result = await userController.SignOut(tokenModel);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task SignOut_RefreshNotMatchAccess()
        {
            var userService = FakeUserServiceFactory.UserSignOut(UserResult.RefreshNotMatchAccess, "Refresh token not match access");
            var tokenModel = new TokenModel(("asd", DateTime.Now), ("sd", DateTime.Now));

            var userController = new UserController(userService);

            var result = await userController.SignOut(tokenModel);

            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public async Task SignOut_NoContent()
        {
            var userService = FakeUserServiceFactory.UserSignOut(UserResult.Error, "DB Error");
            var tokenModel = new TokenModel(("asd", DateTime.Now), ("sd", DateTime.Now));

            var userController = new UserController(userService);

            var result = await userController.SignOut(tokenModel);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
