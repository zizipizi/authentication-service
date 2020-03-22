using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Repositories;
using Authentication.Host.Results.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Authentication.Tests.RepositoryTests.AdminRepositoryTests
{
    public class WhenBlockUser
    {
        [Fact]
        public async Task BlockUser_Ok()
        {
            var authContext = FakeContextFactory.BlockUser_Ok(out var id);

            var adminRepository = new AdminRepository(authContext);

            var result = await adminRepository.BlockUserAsync(id, CancellationToken.None);

            var dbResult = await authContext.Users.FirstOrDefaultAsync(c => c.Id == id);

            result.Value.Should().BeEquivalentTo(AdminRepositoryResult.Ok);
            dbResult.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task BlockUser_NotFound()
        {
            var authContext = FakeContextFactory.BlockUser_UserNotFound();

            var adminRepository = new AdminRepository(authContext);
            var id = authContext.Users.Max(c => c.Id) + 1;

            var user = await authContext.Users.FirstOrDefaultAsync(c => c.Id == id);

            var result = await adminRepository.BlockUserAsync(id, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(AdminRepositoryResult.UserNotFound);
        }
    }
}
