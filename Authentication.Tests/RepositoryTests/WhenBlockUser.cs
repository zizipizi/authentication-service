using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Exceptions;
using Authentication.Data.Models.Domain;
using Authentication.Host.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Authentication.Tests.RepositoryTests
{
    public class WhenBlockUser
    {
        [Fact]
        public async Task BlockUser_Ok()
        {
            var authContext = FakeContextFactory.BlockUser_Ok(out var id);
            var cache = new Mock<IDistributedCache>().Object;

            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object, cache);
            var userRepository = new UserRepository(tokenRepository, authContext, new Mock<ILogger<UserRepository>>().Object);

            await userRepository.BlockUserAsync(id, CancellationToken.None);

            var result = await authContext.Users.FirstOrDefaultAsync(c => c.Id == id);

            result.IsActive.Should().BeFalse();
            //Assert.False(result.IsActive);
        }

        [Fact]
        public async Task BlockUser_EntityException()
        {
            var authContext = FakeContextFactory.BlockUser_EntityException(out var id);
            var logger = new Mock<ILogger<UserRepository>>().Object;
            var cache = new Mock<IDistributedCache>().Object;

            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object, cache);
            var userRepository = new UserRepository(tokenRepository, authContext, logger);

            Func<Task> act = async () => { await userRepository.BlockUserAsync(id+1, CancellationToken.None); };
            
            await act.Should().ThrowAsync<EntityNotFoundException>().WithMessage("User not found");

            //var ex = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await userRepository.BlockUserAsync(2, CancellationToken.None));
            //Assert.Equal("User not found", ex.Message);

        }
    }
}
