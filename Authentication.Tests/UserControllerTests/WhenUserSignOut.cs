using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Results.Enums;
using Authentication.Tests.UserControllerTests.Utils;
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
            var result = await SignOut(UserResult.RefreshTokenExpired, "Refresh token expired");

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task SignOut_RefreshNotMatchAccess()
        {
            var result = await SignOut(UserResult.RefreshNotMatchAccess, "Refresh token not match access");

            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public async Task SignOut_NoContent()
        {
            var result = await SignOut(UserResult.Error, "DB Error");

            Assert.IsType<NoContentResult>(result);
        }

        public async Task<IActionResult> SignOut(UserResult expectationResult, string message = "")
        {
            var userService = FakeUserServiceFactory.UserSignOut(expectationResult, message);
            var tokenModel = FakeModels.FakeTokenModel();

            var userController = new UserController(userService);

            var result = await userController.SignOut(tokenModel);

            return result;
        }
    }
}
