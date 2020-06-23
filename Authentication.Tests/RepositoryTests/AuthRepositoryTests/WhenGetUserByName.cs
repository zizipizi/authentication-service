using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Repositories;
using Authentication.Host.Results.Enums;
using FluentAssertions;
using Xunit;

namespace Authentication.Tests.RepositoryTests.AuthRepositoryTests
{
    public class WhenGetUserByName
    {
        [Fact]
        public async Task GetUserByName_Ok()
        {
            var authContext = FakeContextFactory.GetUserByName_Ok();

            var authRepository = new AuthRepository(authContext);

            var result = await authRepository.GetUserByNameAsync("Login", CancellationToken.None);
            var result2 = await authRepository.GetUserByNameAsync("Login2", CancellationToken.None);

            result.Model.Login.Should().BeEquivalentTo("Login");
            result.Value.Should().BeEquivalentTo(AuthRepositoryResult.Ok);

            result2.Model.Login.Should().BeEquivalentTo("Login2");
            result2.Value.Should().BeEquivalentTo(AuthRepositoryResult.Ok);
        }

        [Fact]
        public async Task GetUserByName_UserNotFound()
        {
            var authContext = FakeContextFactory.GetUserByName_NotFound();

            var authRepository = new AuthRepository(authContext);

            var result = await authRepository.GetUserByNameAsync("Login324", CancellationToken.None);
            var result2 = await authRepository.GetUserByNameAsync("Login24345", CancellationToken.None);

            result.Model.Should().BeNull();
            result.Value.Should().BeEquivalentTo(AuthRepositoryResult.UserNotFound);

            result2.Model.Should().BeNull();
            result2.Value.Should().BeEquivalentTo(AuthRepositoryResult.UserNotFound);
        }

    }
}
