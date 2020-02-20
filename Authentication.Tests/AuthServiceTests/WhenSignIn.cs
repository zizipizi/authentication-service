using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using NSV.Security.JWT;
using NSV.Security.Password;
using Xunit;

namespace Authentication.Tests.AuthServiceTests
{
    public class WhenSignIn
    {
        [Fact]
        public async Task SignIn_Ok()
        {
            var logger = new Mock<ILogger<AuthService>>().Object;
            var passwordService = new Mock<IPasswordService>().Object;
            var jwtService = new Mock<IJwtService>().Object;
            var cache = new Mock<IDistributedCache>().Object;


            var userRepository = FakeRepositoryFactory.SignIn();
            var tokenRepository = FakeRepositoryFactory.AddTokens_Ok();

            var loginModel = new LoginModel
            {
                UserName = "UserName",
                Password = "Password2020"
            };

            var authService = new AuthService(jwtService, passwordService, userRepository, tokenRepository, logger, cache);

            var result = await authService.SignIn(loginModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(AuthResult.Ok);
            //Assert.Equal(AuthResult.Ok, result.Value);
        }

        [Fact]
        public async Task SignIn_ThrowsEntityException()
        {
            var logger = new Mock<ILogger<AuthService>>().Object;
            var passwordService = new Mock<IPasswordService>().Object;
            var jwtService = new Mock<IJwtService>().Object;
            var cache = new Mock<IDistributedCache>().Object;


            var userRepository = FakeRepositoryFactory.SignIn_EntityException();
            var tokenRepository = FakeRepositoryFactory.FakeToken();

            var loginModel = new LoginModel
            {
                UserName = "UserName",
                Password = "Password2020"
            };

            var authService = new AuthService(jwtService, passwordService, userRepository, tokenRepository, logger, cache);

            var result = await authService.SignIn(loginModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(AuthResult.UserNotFound);
            //Assert.Equal(AuthResult.UserNotFound, result.Value);
        }

        [Fact]
        public async Task SignIn_ThrowsException()
        {
            var logger = new Mock<ILogger<AuthService>>().Object;
            var passwordService = new Mock<IPasswordService>().Object;
            var jwtService = new Mock<IJwtService>().Object;
            var cache = new Mock<IDistributedCache>().Object;


            var userRepository = FakeRepositoryFactory.SignIn_Exception();
            var tokenRepository = FakeRepositoryFactory.FakeToken();

            var loginModel = new LoginModel
            {
                UserName = "UserName",
                Password = "Password2020"
            };

            var authService = new AuthService(jwtService, passwordService, userRepository, tokenRepository, logger, cache);

            var result = await authService.SignIn(loginModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(AuthResult.Error);
            //Assert.Equal(AuthResult.Error, result.Value);
        }
    }
}
