using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Entities;
using Authentication.Host.Repositories;
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
            var authContext = FakeContextFactory.AddTokensWithRefresh();
            var logger = new Mock<ILogger<UserRepository>>().Object;

            var userRepository = new UserRepository(authContext, logger);

            var refreshJti = "123123podaps123123";
            var tokenModel = new TokenModel(
                ("asdakasdjaksjdkad", DateTime.Now.AddMinutes(15), "1231123123"),
                ("sdfk;sldkfsdl;fksdf0", DateTime.Now.AddMinutes(30), refreshJti)
                );

            var jwtToken= new JwtTokenResult(model: tokenModel, userId: "1");

            await userRepository.AddTokensAsync(jwtToken, CancellationToken.None);

            var refreshToken = authContext.RefreshTokens.FirstOrDefault(c => c.Jti == refreshJti);
            var accessToken = authContext.AccessTokens.FirstOrDefault(c => c.RefreshToken == refreshToken);

            Assert.NotNull(refreshToken);
            Assert.Equal(refreshJti, refreshToken.Jti);
            Assert.NotNull(accessToken);
        }

        [Fact]
        public async Task AddTokenWithoutRefresh()
        {
            var authContext = FakeContextFactory.AddTokenWithoutRefresh();
            var logger = new Mock<ILogger<UserRepository>>().Object;
            var refreshJti = "123123podaps123123";

            var tokenModel = new TokenModel(
                ("asdakasdjaksjdkad", DateTime.Now.AddMinutes(15), "1231123123"),
                ("sdfk;sldkfsdl;fksdf0", DateTime.Now.AddMinutes(30), refreshJti)
            );

            var jwtToken = new JwtTokenResult(model: tokenModel, userId: "1");

            var refreshToken = new RefreshTokenEntity
            {
                Token = "asdasdqwjqwioeqowieu",
                Created = DateTime.UtcNow,
                Expired = DateTime.Now.AddMinutes(30),
                Jti = jwtToken.Tokens.RefreshToken.Jti,
                IsBlocked = false,
                UserId = long.Parse(jwtToken.UserId)
            };

            authContext.RefreshTokens.Add(refreshToken);
            authContext.SaveChanges();

            var userRepository = new UserRepository(authContext, logger);

            await userRepository.AddTokensAsync(jwtToken, CancellationToken.None);

            var accessToken = authContext.AccessTokens.FirstOrDefault(c => c.RefreshToken == refreshToken);

            Assert.NotNull(accessToken);
            Assert.Equal("asdakasdjaksjdkad", accessToken.Token);
        }
    }
}
