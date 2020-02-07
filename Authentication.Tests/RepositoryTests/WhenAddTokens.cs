﻿using System;
using System.Linq;
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
            var logger = new Mock<ILogger<TokenRepository>>().Object;

            var tokenRepository = new TokenRepository(authContext, logger);
            
            var refreshJti = "123123podaps123123";
            var tokenModel = new TokenModel(
                ("asdakasdjaksjdkad", DateTime.Now.AddMinutes(15), "1231123123"),
                ("sdfk;sldkfsdl;fksdf0", DateTime.Now.AddMinutes(30), refreshJti)
                );

            await tokenRepository.AddTokensAsync(1, tokenModel, CancellationToken.None);

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
            var logger = new Mock<ILogger<TokenRepository>>().Object;
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
                UserId = 1
            };

            authContext.RefreshTokens.Add(refreshToken);
            authContext.SaveChanges();

            var tokenRepository = new TokenRepository(authContext, logger);

            await tokenRepository.AddTokensAsync(1, tokenModel, CancellationToken.None);

            var accessToken = authContext.AccessTokens.FirstOrDefault(c => c.RefreshToken == refreshToken);

            Assert.NotNull(accessToken);
            Assert.Equal("asdakasdjaksjdkad", accessToken.Token);
        }
    }
}
