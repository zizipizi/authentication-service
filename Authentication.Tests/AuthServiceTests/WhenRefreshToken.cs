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

            var fakeTokenRepository = FakeRepositoryFactory.RefreshToken_Ok();
            var fakeUserRepository = FakeRepositoryFactory.FakeUser();

            var authService = new AuthService(jwtService, passwordService, fakeUserRepository, fakeTokenRepository, logger,cache);

            var bodyTokenModel = new BodyTokenModel
            {
                AccessToken = "adasdasd",
                RefreshToken = "asdasdfsdf"
            };

            var result = await authService.RefreshToken(bodyTokenModel, CancellationToken.None);

            Assert.Equal(AuthResult.Ok, result.Value);
        }

        [Fact]
        public async Task RefrehToken_Error()
        {
            var logger = new Mock<ILogger<AuthService>>().Object;
            var passwordService = new Mock<IPasswordService>().Object;
            var jwtService = new Mock<IJwtService>().Object;
            var cache = new Mock<IDistributedCache>().Object;

            var fakeTokenRepository = FakeRepositoryFactory.CheckRefreshToken_Error();
            var fakeUserRepository = FakeRepositoryFactory.FakeUser();

            var authService = new AuthService(jwtService, passwordService, fakeUserRepository, fakeTokenRepository, logger, cache);

            var bodyTokenModel = new BodyTokenModel
            {
                AccessToken = "adasdasd",
                RefreshToken = "asdasdfsdf"
            };

            var result = await authService.RefreshToken(bodyTokenModel, CancellationToken.None);

            Assert.Equal(AuthResult.TokenValidationProblem, result.Value);
        }
    }
}
