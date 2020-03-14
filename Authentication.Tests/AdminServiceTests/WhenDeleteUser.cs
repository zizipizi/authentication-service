using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Authentication.Tests.AdminServiceTests.Utils;
using FluentAssertions;
using Moq;
using NSV.Security.Password;
using Processing.Kafka.Producer;
using Xunit;

namespace Authentication.Tests.AdminServiceTests
{
    public class WhenDeleteUser
    {
        [Fact]
        public async Task DeleteUser_Success()
        {
            var id = 1;
            var passwordService = new Mock<IPasswordService>().Object;
            var cacheRepository = FakeCacheRepositoryFactory.FakeAddRefreshTokensToBlacklistAsync(CacheRepositoryResult.Ok);

            var kafka = new Mock<IProducerFactory<long, string>>();
            var producer = new Mock<IKafkaProducer<long, string>>();

            kafka.Setup(c => c.GetOrCreate(It.IsAny<string>(), null))
                .Returns(producer.Object);


            var userRepo = FakeAdminRepositoryFactory.FakeDeleteUser(AdminRepositoryResult.Ok);
            var userService = new AdminService(userRepo, cacheRepository, passwordService, kafka.Object);

            var result = await userService.DeleteUserAsync(id, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteUser_NotFound()
        {
            var id = 1;
            var passwordService = new Mock<IPasswordService>().Object;

            var kafka = new Mock<IProducerFactory<long, string>>();
            var producer = new Mock<IKafkaProducer<long, string>>();

            kafka.Setup(c => c.GetOrCreate(It.IsAny<string>(), null))
                .Returns(producer.Object);

            var cacheRepository = FakeCacheRepositoryFactory.FakeAddRefreshTokensToBlacklistAsync(CacheRepositoryResult.Ok);
            var userRepo = FakeAdminRepositoryFactory.FakeDeleteUser(AdminRepositoryResult.UserNotFound);
            var userService = new AdminService(userRepo, cacheRepository, passwordService, kafka.Object);

            var result = await userService.DeleteUserAsync(id, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteUser_ServiceUnavailable_FromCacheRepository()
        {
            var id = 1;
            var passwordService = new Mock<IPasswordService>().Object;

            var cacheRepository = FakeCacheRepositoryFactory.FakeAddRefreshTokensToBlacklistAsync(CacheRepositoryResult.Error);
            var userRepo = FakeAdminRepositoryFactory.FakeDeleteUser(AdminRepositoryResult.Ok);

            var kafka = new Mock<IProducerFactory<long, string>>();
            var producer = new Mock<IKafkaProducer<long, string>>();

            kafka.Setup(c => c.GetOrCreate(It.IsAny<string>(), null))
                .Returns(producer.Object);

            var userService = new AdminService(userRepo, cacheRepository, passwordService, kafka.Object);

            var result = await userService.DeleteUserAsync(id, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.ServiceUnavailable);
        }

        [Fact]
        public async Task DeleteUser_ServiceUnavailable_FromAdminRepository()
        {
            var id = 1;
            var passwordService = new Mock<IPasswordService>().Object;

            var cacheRepository = FakeCacheRepositoryFactory.FakeAddRefreshTokensToBlacklistAsync(CacheRepositoryResult.Ok);
            var userRepo = FakeAdminRepositoryFactory.FakeDeleteUser(AdminRepositoryResult.Error);

            var kafka = new Mock<IProducerFactory<long, string>>();
            var producer = new Mock<IKafkaProducer<long, string>>();

            kafka.Setup(c => c.GetOrCreate(It.IsAny<string>(), null))
                .Returns(producer.Object);

            var userService = new AdminService(userRepo, cacheRepository, passwordService, kafka.Object);

            var result = await userService.DeleteUserAsync(id, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.ServiceUnavailable);
        }

    }
}
