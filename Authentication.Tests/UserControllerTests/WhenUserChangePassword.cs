using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Tests.UserControllerTests.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
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

            var contextAccessor = new Mock<IHttpContextAccessor>().Object;
            var logger = new Mock<ILogger<UserController>>().Object;

            var userService = FakeUserServiceFactory.UserChangePassword(UserResult.Ok, tokenModel, "");
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
                    Request = { Headers = { ["Authorization"] = "asdk"}}
                }
            };

            var result = await userController.ChangePassword(changePassModel);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UserChangePassword_WrongPassword()
        {
            var tokenModel = FakeModels.FakeTokenModel();
                var changePassModel = FakeModels.FakePasswords();

                var contextAccessor = new Mock<IHttpContextAccessor>().Object;
                var logger = new Mock<ILogger<UserController>>().Object;

                var userService = FakeUserServiceFactory.UserChangePassword(UserResult.WrongPassword, tokenModel, "");
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

            var result = await userController.ChangePassword(changePassModel);

            Assert.IsType<NotFoundObjectResult>(result);
        }

    }
}
