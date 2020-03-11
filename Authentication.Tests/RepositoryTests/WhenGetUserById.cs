//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Authentication.Data.Exceptions;
//using Authentication.Data.Models.Entities;
//using Authentication.Host.Repositories;
//using Elasticsearch.Net.Specification.IngestApi;
//using FluentAssertions;
//using Microsoft.Extensions.Caching.Distributed;
//using Microsoft.Extensions.Logging;
//using Moq;
//using Xunit;

//namespace Authentication.Tests.RepositoryTests
//{
//    public class WhenGetUserById
//    {
//        [Fact]
//        public async Task GetUserById_Ok()
//        {
//            var authContext = FakeContextFactory.GetUserById_Ok(out var id);
//            var logger = new Mock<ILogger<UserRepository>>().Object;
//            var cache = new Mock<IDistributedCache>().Object;

//            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object, cache);
//            var userRepository = new UserRepository(tokenRepository, authContext, logger);

//            var result = await userRepository.GetUserByIdAsync(id, CancellationToken.None);

//            result.Id.Should().Be(id);
//            result.Login.Should().BeEquivalentTo("Login");

//            //Assert.Equal(1, result.Id);
//            //Assert.Equal("Login", result.Login);
//        }

//        [Fact]
//        public async Task GetUserById_EntityException()
//        {
//            var authContext = FakeContextFactory.GetUserById_EntityException(out var id);
//            var logger = new Mock<ILogger<UserRepository>>().Object;
//            var cache = new Mock<IDistributedCache>().Object;

//            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object, cache);
//            var userRepository = new UserRepository(tokenRepository, authContext, logger);

//            Func<Task> act = async () => await userRepository.GetUserByIdAsync(id+1, CancellationToken.None);
//            await act.Should().ThrowAsync<EntityNotFoundException>().WithMessage("User not found");

//            //var ex = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await userRepository.GetUserByIdAsync(3, CancellationToken.None));
//            //Assert.Equal("User not found", ex.Message);
//        }
//    }
//}
