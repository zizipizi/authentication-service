using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Repositories;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Authentication.Tests.AdminServiceTests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Moq;
using NSV.Security.JWT;
using NSV.Security.Password;
using Xunit;

namespace Authentication.Tests.AdminServiceTests
{
    public class WhenBlockUser
    {
        [Fact]
        public async Task BlockUser_Success()
        {
            var id = 1;
            var passwordService = new Mock<IPasswordService>().Object;

            var cacheRepository = FakeCacheRepositoryFactory.FakeAddRefreshTokensToBlacklistAsync(CacheRepositoryResult.Ok);
            var adminRepo = FakeAdminRepositoryFactory.FakeBlockUser(AdminRepositoryResult.Ok);

            var userService = new AdminService(adminRepo, cacheRepository, passwordService);

            var result = await userService.BlockUserAsync(id, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [Fact]
        public async Task BlockUser_NotFound()
        {
            var id = 1;
            var passwordService = new Mock<IPasswordService>().Object;

            var cacheRepository = FakeCacheRepositoryFactory.FakeAddRefreshTokensToBlacklistAsync(CacheRepositoryResult.Ok);
            var adminRepo = FakeAdminRepositoryFactory.FakeBlockUser(AdminRepositoryResult.UserNotFound);

            var userService = new AdminService(adminRepo, cacheRepository, passwordService);

            var result = await userService.BlockUserAsync(id, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task BlockUser_ServiceUnavailable_FromCacheRepository()
        {
            var id = 1;
            var passwordService = new Mock<IPasswordService>().Object;
            var adminRepo = FakeAdminRepositoryFactory.FakeBlockUser(AdminRepositoryResult.Ok);

            var cacheRepository = FakeCacheRepositoryFactory.FakeAddRefreshTokensToBlacklistAsync(CacheRepositoryResult.Error);

            var userService = new AdminService(adminRepo, cacheRepository, passwordService);

            var result = await userService.BlockUserAsync(id, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.ServiceUnavailable);
        }

        [Fact]
        public async Task BlockUser_ServiceUnavailable_FromAdminRepository()
        {
            var id = 1;
            var passwordService = new Mock<IPasswordService>().Object;

            var cacheRepository = FakeCacheRepositoryFactory.FakeAddRefreshTokensToBlacklistAsync(CacheRepositoryResult.Ok);
            var adminRepo = FakeAdminRepositoryFactory.FakeBlockUser(AdminRepositoryResult.Error);

            var userService = new AdminService(adminRepo, cacheRepository, passwordService);

            var result = await userService.BlockUserAsync(id, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.ServiceUnavailable);
        }
    }
}
