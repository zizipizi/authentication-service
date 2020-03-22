using System;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Data.Models.Entities;
using Authentication.Host.Repositories;
using Authentication.Host.Results.Enums;
using FluentAssertions;
using NSV.Security.JWT;
using Xunit;

namespace Authentication.Tests.RepositoryTests.AuthRepositoryTests
{
    public class WhenAddTokens
    {
        [Fact]
        public async Task AddTokens_Ok()
        {
            var authContext = FakeContextFactory.AddTokensWithRefresh(out long id);

            var refreshJti = "123123podaps123123";
            var tokenModel = new TokenModel(
                ("asdakasdjaksjdkad", DateTime.Now.AddMinutes(15), "1231123123"),
                ("sdfk;sldkfsdl;fksdf0", DateTime.Now.AddMinutes(30), refreshJti)
                );

            var authRepository = new AuthRepository(authContext);

            var addTokenResult = await authRepository.AddTokensAsync(id, tokenModel);

            var refreshToken = authContext.RefreshTokens.FirstOrDefault(c => c.Jti == refreshJti);
            var accessToken = authContext.AccessTokens.FirstOrDefault(c => c.RefreshToken == refreshToken);

            refreshToken.Should().NotBeNull();
            refreshToken.Jti.Should().BeEquivalentTo(refreshJti);
            accessToken.Should().NotBeNull();
            addTokenResult.Value.Should().BeEquivalentTo(AuthRepositoryResult.Ok);
        }

        [Fact]
        public async Task AddTokens_WithoutRefresh()
        {
            var authContext = FakeContextFactory.AddTokenWithoutRefresh(out var id);

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

            var authRepository = new AuthRepository(authContext);

            var addTokensResult = await authRepository.AddTokensAsync(id, tokenModel);

            var accessToken = authContext.AccessTokens.FirstOrDefault(c => c.RefreshToken == refreshToken);

            accessToken.Should().NotBeNull();
            accessToken.Token.Should().BeEquivalentTo("asdakasdjaksjdkad");
            addTokensResult.Value.Should().BeEquivalentTo(AuthRepositoryResult.Ok);
        }
    }
}
