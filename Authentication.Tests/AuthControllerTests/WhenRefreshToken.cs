using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Tests.AuthControllerTests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Authentication.Tests.AuthControllerTests
{
    public class WhenRefreshToken
    {
        [Fact]
        public async Task RefreshToken_Ok()
        {
            var bodyToken = new BodyTokenModel
            {
                AccessToken = "ajsdasjdiaosd",
                RefreshToken = "klfodkfokdok"
            };

            var authService = FakeAuthServiceFactory.FakeRefreshToken(HttpStatusCode.OK, bodyToken);

            var authController = new AuthController(authService);

            var result = await authController.RefreshToken(bodyToken, CancellationToken.None);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task RefreshToken_TokenIsBlocked()
        {
            var bodyToken = new BodyTokenModel
            {
                AccessToken = "ajsdasjdiaosd",
                RefreshToken = "klfodkfokdok"
            };

            var authService = FakeAuthServiceFactory.FakeRefreshToken(HttpStatusCode.Unauthorized, bodyToken);
            var logger = new Mock<ILogger<AuthController>>().Object;


            var authController = new AuthController(authService, logger);

            var result = await authController.RefreshToken(bodyToken, CancellationToken.None);

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task RefreshToken_ServiceUnavailable()
        {
            var authService = FakeAuthServiceFactory.FakeRefreshToken(HttpStatusCode.ServiceUnavailable);

            var authController = new AuthController(authService);

            var bodyToken = new BodyTokenModel
            {
                AccessToken = "ajsdasjdiaosd",
                RefreshToken = "klfodkfokdok"
            };

            var result = await authController.RefreshToken(bodyToken, CancellationToken.None);

            result.Should().BeOfType(typeof(StatusCodeResult));
        }

    }
}
