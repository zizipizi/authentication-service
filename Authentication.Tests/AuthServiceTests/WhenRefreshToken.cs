using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using NSV.Security.JWT;
using NSV.Security.Password;
using Xunit;

namespace Authentication.Tests.AuthServiceTests
{
    public class WhenRefreshToken
    {
        [Fact]
        public async Task RefrehToken_OK()
        {
            var logger = new Mock<ILogger<AuthService>>().Object;
            var passwordService = new Mock<IPasswordService>().Object;
            var jwtService = new Mock<IJwtService>().Object;
            var cache = new Mock<IDistributedCache>().Object;

            var fakeUserrepository = FakeRepositoryFactory.RefreshToken_Ok();


            var authService = new AuthService(jwtService, passwordService, fakeUserrepository, logger,cache);

            var bodyTokenModel = new BodyTokenModel
            {
                AccessToken = "adasdasd",
                RefreshToken = "asdasdfsdf"
            };

            var result = await authService.RefreshToken(bodyTokenModel, CancellationToken.None);

            Assert.Equal(AuthResult.Ok, result.Value);
        }

        [Fact]
        public async Task RefrehToken_ThrowsEntityException()
        {
            var logger = new Mock<ILogger<AuthService>>().Object;
            var passwordService = new Mock<IPasswordService>().Object;
            var jwtService = new Mock<IJwtService>().Object;
            var cache = new Mock<IDistributedCache>().Object;

            var fakeUserrepository = FakeRepositoryFactory.RefreshToken_EntityException();


            var authService = new AuthService(jwtService, passwordService, fakeUserrepository, logger, cache);

            var bodyTokenModel = new BodyTokenModel
            {
                AccessToken = "adasdasd",
                RefreshToken = "asdasdfsdf"
            };

            var result = await authService.RefreshToken(bodyTokenModel, CancellationToken.None);

            Assert.Equal(AuthResult.Error, result.Value);
        }

        [Fact]
        public async Task RefrehToken_ThrowsException()
        {
            var logger = new Mock<ILogger<AuthService>>().Object;
            var passwordService = new Mock<IPasswordService>().Object;
            var jwtService = new Mock<IJwtService>().Object;
            var cache = new Mock<IDistributedCache>().Object;

            var fakeUserrepository = FakeRepositoryFactory.RefreshToken_Exception();


            var authService = new AuthService(jwtService, passwordService, fakeUserrepository, logger, cache);

            var bodyTokenModel = new BodyTokenModel
            {
                AccessToken = "adasdasd",
                RefreshToken = "asdasdfsdf"
            };

            var result = await authService.RefreshToken(bodyTokenModel, CancellationToken.None);

            Assert.Equal(AuthResult.Error, result.Value);
        }
    }
}
