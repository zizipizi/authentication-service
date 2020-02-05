using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Tests.AuthControllerTests.Utils;
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
            var authService = FakeAuthServiceFactory.FakeRefreshToken(AuthResult.Ok);
            var logger = new Mock<ILogger<AuthController>>().Object;


            var authController = new AuthController(authService, logger);

            var bodyToken = new BodyTokenModel
            {
                AccessToken = "ajsdasjdiaosd",
                RefreshToken = "klfodkfokdok"
            };

            var result = await authController.RefreshToken(bodyToken, CancellationToken.None);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task RefreshToken_TokenvalidationProblem()
        {
            var authService = FakeAuthServiceFactory.FakeRefreshToken(AuthResult.TokenValidationProblem);
            var logger = new Mock<ILogger<AuthController>>().Object;


            var authController = new AuthController(authService, logger);

            var bodyToken = new BodyTokenModel
            {
                AccessToken = "ajsdasjdiaosd",
                RefreshToken = "klfodkfokdok"
            };

            var result = await authController.RefreshToken(bodyToken, CancellationToken.None);

            Assert.IsType<ConflictObjectResult>(result);
        }


        [Fact]
        public async Task RefreshToken_TokenExpired()
        {
            var authService = FakeAuthServiceFactory.FakeRefreshToken(AuthResult.TokenExpired);
            var logger = new Mock<ILogger<AuthController>>().Object;


            var authController = new AuthController(authService, logger);

            var bodyToken = new BodyTokenModel
            {
                AccessToken = "ajsdasjdiaosd",
                RefreshToken = "klfodkfokdok"
            };

            var result = await authController.RefreshToken(bodyToken, CancellationToken.None);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task RefreshToken_ErrorWhileRefresh()
        {
            var authService = FakeAuthServiceFactory.FakeRefreshToken(AuthResult.Error);
            var logger = new Mock<ILogger<AuthController>>().Object;


            var authController = new AuthController(authService, logger);

            var bodyToken = new BodyTokenModel
            {
                AccessToken = "ajsdasjdiaosd",
                RefreshToken = "klfodkfokdok"
            };

            var result = await authController.RefreshToken(bodyToken, CancellationToken.None);

            Assert.IsType<BadRequestObjectResult>(result);
        }



    }
}
