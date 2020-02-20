using Authentication.Tests.AuthControllerTests.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Authentication.Tests.AuthControllerTests
{
    public class WhenSignIn
    {
        [Fact]
        public async Task SignIn_Ok()
        {
            var authService = FakeAuthServiceFactory.FakeSignIn(AuthResult.Ok);
            var logger = new Mock<ILogger<AuthController>>().Object;


            var authController = new AuthController(authService, logger);

            var loginModel = new LoginModel
            {
                UserName = "Terminator",
                Password = "Terminator2013"
            };

            var result = await authController.SignIn(loginModel, CancellationToken.None);

            result.Should().BeOfType<OkObjectResult>();
            //Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task SignIn_NotFound()
        {
            var authService = FakeAuthServiceFactory.FakeSignIn(AuthResult.UserNotFound);
            var logger = new Mock<ILogger<AuthController>>().Object;


            var authController = new AuthController(authService, logger);

            var loginModel = new LoginModel
            {
                UserName = "Terminator",
                Password = "Terminator2013"
            };

            var result = await authController.SignIn(loginModel, CancellationToken.None);

            result.Should().BeOfType<NotFoundObjectResult>();
            //Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task SignIn_UserBlocked()
        {
            var authService = FakeAuthServiceFactory.FakeSignIn(AuthResult.UserBlocked);
            var logger = new Mock<ILogger<AuthController>>().Object;


            var authController = new AuthController(authService, logger);
            ;
            var loginModel = new LoginModel
            {
                UserName = "Terminator",
                Password = "Terminator2013"
            };
            var res = new StatusCodeResult(403);
            var result = await authController.SignIn(loginModel, CancellationToken.None);

            result.Should().BeOfType<UnauthorizedObjectResult>();
            //Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task SignIn_Error()
        {
            var authService = FakeAuthServiceFactory.FakeSignIn(AuthResult.Error);
            var logger = new Mock<ILogger<AuthController>>().Object;


            var authController = new AuthController(authService, logger);

            var loginModel = new LoginModel
            {
                UserName = "Terminator",
                Password = "Terminator2013"
            };

            var result = await authController.SignIn(loginModel, CancellationToken.None);

            result.Should().BeOfType<BadRequestObjectResult>();
            //Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
