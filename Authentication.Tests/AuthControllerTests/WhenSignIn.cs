using Authentication.Tests.AuthControllerTests.Utils;
using System;
using System.Collections.Generic;
using System.Net;
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
            var loginModel = new LoginModel
            {
                UserName = "Terminator",
                Password = "Terminator2013"
            };

            var bodyTokenModel = new BodyTokenModel
            {
                AccessToken = "asdasdasdasd",
                RefreshToken = "asdasddawqe"
            };

            var authService = FakeAuthServiceFactory.FakeSignIn(HttpStatusCode.OK, bodyTokenModel);

            var authController = new AuthController(authService);

            var result = await authController.SignIn(loginModel, CancellationToken.None);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task SignIn_NotFound()
        {
            var authService = FakeAuthServiceFactory.FakeSignIn(HttpStatusCode.NotFound);

            var authController = new AuthController(authService);

            var loginModel = new LoginModel
            {
                UserName = "Terminator",
                Password = "Terminator2013"
            };

            var result = await authController.SignIn(loginModel, CancellationToken.None);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task SignIn_UserBlocked()
        {
            var authService = FakeAuthServiceFactory.FakeSignIn(HttpStatusCode.Unauthorized);
            var logger = new Mock<ILogger<AuthController>>().Object;


            var authController = new AuthController(authService, logger);
            ;
            var loginModel = new LoginModel
            {
                UserName = "Terminator",
                Password = "Terminator2013"
            };

            var result = await authController.SignIn(loginModel, CancellationToken.None);

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task SignIn_ServiceUnavailable()
        {
            var authService = FakeAuthServiceFactory.FakeSignIn(HttpStatusCode.ServiceUnavailable);

            var authController = new AuthController(authService);

            var loginModel = new LoginModel
            {
                UserName = "Terminator",
                Password = "Terminator2013"
            };

            var result = await authController.SignIn(loginModel, CancellationToken.None);

            result.Should().BeOfType(typeof(StatusCodeResult));
        }
    }
}
