using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Repositories;
using Authentication.Host.Results.Enums;
using FluentAssertions;
using Xunit;

namespace Authentication.Tests.RepositoryTests.UserRepositoryTests
{
    public class WhenGetUserById
    {
        [Fact]
        public async Task WhenGetUserById_Ok()
        {
            var authContext = FakeContextFactory.GetUserById_Ok(out var id);

            var userRepository = new UserRepository(authContext);

            var result = await userRepository.GetUserByIdAsync(id, CancellationToken.None);

            result.Model.Id.Should().Be(id);
            result.Model.Login.Should().BeEquivalentTo("Login");

            result.Value.Should().BeEquivalentTo(UserRepositoryResult.Ok);
        }

        [Fact]
        public async Task WhenGetUserById_NotFound()
        {
            var authContext = FakeContextFactory.GetUserById_EntityException(out var id);

            var userRepository = new UserRepository(authContext);

            var userResult = await userRepository.GetUserByIdAsync(id + 1, CancellationToken.None);

            userResult.Value.Should().BeEquivalentTo(UserRepositoryResult.UserNotFound);
            userResult.Model.Should().BeNull();
        }
    }
}
