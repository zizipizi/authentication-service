//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Authentication.Data.Exceptions;
//using Authentication.Host.Repositories;
//using FluentAssertions;
//using Microsoft.Extensions.Caching.Distributed;
//using Microsoft.Extensions.Logging;
//using Moq;
//using Xunit;

//namespace Authentication.Tests.RepositoryTests
//{
//    public class WhenGetUserByName
//    {
//        [Fact]
//        public async Task GetUserByName_Ok()
//        {
//            var authContext = FakeContextFactory.GetUserByName_Ok();
//            var logger = new Mock<ILogger<UserRepository>>().Object;
//            var cache = new Mock<IDistributedCache>().Object;

//            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object, cache);
//            var userRepository = new UserRepository(tokenRepository, authContext, logger);

//            var result = await userRepository.GetUserByNameAsync("Login", CancellationToken.None);
//            var result2 = await userRepository.GetUserByNameAsync("Login2", CancellationToken.None);

//            result.Login.Should().BeEquivalentTo("Login");
//            result2.Login.Should().BeEquivalentTo("Login2");

//            //Assert.Equal("Login", result.Login);
//            //Assert.Equal("Login2", result2.Login);
//        }

//        [Fact]
//        public async Task GetUserByName_EntityException()
//        {
//            var authContext = FakeContextFactory.GetUserByName_Ok();
//            var logger = new Mock<ILogger<UserRepository>>().Object;
//            var cache = new Mock<IDistributedCache>().Object;

//            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object, cache);
//            var userRepository = new UserRepository(tokenRepository, authContext, logger);

//            var result = userRepository.GetUserByNameAsync("Login4", CancellationToken.None);
//            var result2= userRepository.GetUserByNameAsync("Login5", CancellationToken.None);

//            Func<Task> act = async () => await result;
//            Func<Task> act2 = async () => await result2;

//            await act.Should().ThrowAsync<EntityNotFoundException>().WithMessage("User not found");
//            await act2.Should().ThrowAsync<EntityNotFoundException>().WithMessage("User not found");

//            //var ex = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await result);
//            //Assert.Equal("User not found", ex.Message);
//            //var ex2 = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await result2);
//            //Assert.Equal("User not found", ex.Message);
//        }
//    }
//}
