using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Repositories;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Authentication.Tests.UserServiceTests.Utils;
using Confluent.Kafka;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NSV.Security.JWT;
using NSV.Security.Password;
using Processing.Kafka.Producer;
using Processing.Kafka.Protobuf;
using Xunit;

namespace Authentication.Tests.UserServiceTests
{
    public class WhenSignOut
    {
        [Fact]
        public async Task SignOut_Ok()
        {
            var passwordService = FakePasswordServiceFactory.FakeValidate(PasswordValidateResult.ValidateResult.Invalid);

            var jwtService = FakeJwtServiceFactory.FakeIssueAccessToken(JwtTokenResult.TokenResult.Ok);

            var cacheRepo = FakeCacheRepositoryFactory.FakeCacheRepository(CacheRepositoryResult.IsNotBlocked, CacheRepositoryResult.Ok);

            var keySerializer = new ProtobufSerializer<int>();
            var valueSerializer = new ProtobufSerializer<string>();

            var producerConfigs = new Mock<IOptions<ProducerConfigs>>();
            var kaf = new ProducerFactory<int, string>(keySerializer, valueSerializer, producerConfigs.Object);

            
            var kafka = new Mock<IProducerFactory<int, string>>();

            var userRepoOptions = new UserRepoOptionsBuilder()
                .GetUserByIdReturns(UserRepositoryResult.Ok)
                .UpdateUserPasswordsReturns(UserRepositoryResult.Ok)
                .BlockAllRefreshTokensReturns(UserRepositoryResult.Ok)
                .BlockRefreshTokenReturns(UserRepositoryResult.Ok)
                .Build();

            var userRepo = FakeUserRepositoryFactory.FakeUserRepo(userRepoOptions);

            var userService = new UserService(userRepo, passwordService, jwtService, cacheRepo, kaf);

            var result = await userService.SignOutAsync(1, "asdasd", CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task SignOut_BadRequest()
        {
            var passwordService = FakePasswordServiceFactory.FakeValidate(PasswordValidateResult.ValidateResult.Invalid);

            var jwtService = FakeJwtServiceFactory.FakeIssueAccessToken(JwtTokenResult.TokenResult.Ok);

            var cacheRepo = FakeCacheRepositoryFactory.FakeCacheRepository(CacheRepositoryResult.IsNotBlocked, CacheRepositoryResult.Ok);

            var keySerializer = new ProtobufSerializer<int>();
            var valueSerializer = new ProtobufSerializer<string>();

            var producerConfigs = new Mock<IOptions<ProducerConfigs>>();
            var kaf = new ProducerFactory<int, string>(keySerializer, valueSerializer, producerConfigs.Object);


            var kafka = new Mock<IProducerFactory<int, string>>();

            var userRepoOptions = new UserRepoOptionsBuilder()
                .GetUserByIdReturns(UserRepositoryResult.Ok)
                .UpdateUserPasswordsReturns(UserRepositoryResult.Ok)
                .BlockAllRefreshTokensReturns(UserRepositoryResult.Ok)
                .BlockRefreshTokenReturns(UserRepositoryResult.Error)
                .Build();

            var userRepo = FakeUserRepositoryFactory.FakeUserRepo(userRepoOptions);

            var userService = new UserService(userRepo, passwordService, jwtService, cacheRepo, kaf);

            var result = await userService.SignOutAsync(1, "asdasd", CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }

    }
}
