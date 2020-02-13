using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Results.Enums;
using Authentication.Tests.UserControllerTests.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
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
            var tokenModel = FakeModels.FakeTokenModel();

            var logger = new Mock<ILogger<UserController>>().Object;


            var userService = FakeUserServiceFactory.UserSignOut(UserResult.Ok, "");
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "asd"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(JwtRegisteredClaimNames.Jti, "jti333")
                }
            ));


            var userController = new UserController(userService, logger);

            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user,
                    Request = { Headers = { ["Authorization"] = "asdk" } }
                }
            };

            
            var result = await userController.SignOut(tokenModel, CancellationToken.None);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task SignOut_BadRequest()
        {
            var tokenModel = FakeModels.FakeTokenModel();

            var logger = new Mock<ILogger<UserController>>().Object;


            var userService = FakeUserServiceFactory.UserSignOut(UserResult.Error, "");
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "asd"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Role, "User"),
                }
            ));


            var userController = new UserController(userService, logger);

            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user,
                    Request = { Headers = { ["Authorization"] = "asdk" } }
                }
            };

            var result = await userController.SignOut(tokenModel, CancellationToken.None);

            Assert.IsType<OkObjectResult>(result);
        }

    }
}
