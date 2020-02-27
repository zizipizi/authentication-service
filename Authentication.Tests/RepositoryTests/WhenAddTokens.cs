using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Entities;
using Authentication.Host.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using NSV.Security.JWT;
using Xunit;

namespace Authentication.Tests.RepositoryTests
{
    public class WhenAddTokens
    {
        [Fact]
        public async Task AddTokensWithRefresh()
        {
            var authContext = FakeContextFactory.AddTokensWithRefresh(out long id);
            var logger = new Mock<ILogger<TokenRepository>>().Object;
            var cache = new Mock<IDistributedCache>().Object;

            var tokenRepository = new TokenRepository(authContext, logger, cache);
            
            var refreshJti = "123123podaps123123";
            var tokenModel = new TokenModel(
                ("asdakasdjaksjdkad", DateTime.Now.AddMinutes(15), "1231123123"),
                ("sdfk;sldkfsdl;fksdf0", DateTime.Now.AddMinutes(30), refreshJti)
                );

            await tokenRepository.AddTokensAsync(id, tokenModel, CancellationToken.None);

            var refreshToken = authContext.RefreshTokens.FirstOrDefault(c => c.Jti == refreshJti);
            var accessToken = authContext.AccessTokens.FirstOrDefault(c => c.RefreshToken == refreshToken);

            refreshToken.Should().NotBeNull();
            refreshToken.Jti.Should().BeEquivalentTo(refreshJti);
            accessToken.Should().NotBeNull();
            //Assert.NotNull(refreshToken);
            //Assert.Equal(refreshJti, refreshToken.Jti);
            //Assert.NotNull(accessToken);
        }

        [Fact]
        public async Task AddTokenWithoutRefresh()
        {
            var authContext = FakeContextFactory.AddTokenWithoutRefresh(out var id);
            var logger = new Mock<ILogger<TokenRepository>>().Object;
            var cache = new Mock<IDistributedCache>().Object;
            var refreshJti = "123123podaps123123";

            var tokenModel = new TokenModel(
                ("asdakasdjaksjdkad", DateTime.Now.AddMinutes(15), "1231123123"),
                ("sdfk;sldkfsdl;fksdf0", DateTime.Now.AddMinutes(30), refreshJti)
            );

            var refreshToken = new RefreshTokenEntity
            {
                Token = "sdfk;sldkfsdl;fksdf0",
                Created = DateTime.UtcNow,
                Expired = DateTime.Now.AddMinutes(30),
                Jti = refreshJti,
                IsBlocked = false,
                UserId = id
            };

            authContext.RefreshTokens.Add(refreshToken);
            authContext.SaveChanges();

            var tokenRepository = new TokenRepository(authContext, logger, cache);

            await tokenRepository.AddTokensAsync(id, tokenModel, CancellationToken.None);

            var accessToken = authContext.AccessTokens.FirstOrDefault(c => c.RefreshToken == refreshToken);

            accessToken.Should().NotBeNull();
            accessToken.Token.Should().BeEquivalentTo("asdakasdjaksjdkad");
            //Assert.NotNull(accessToken);
            //Assert.Equal("asdakasdjaksjdkad", accessToken.Token);
        }
    }
}
