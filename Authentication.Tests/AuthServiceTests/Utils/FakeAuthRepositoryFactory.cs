using System;
using System.Threading;
using Authentication.Data.Models.Domain;
using Authentication.Host.Repositories;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Moq;
using NSV.Security.JWT;

namespace Authentication.Tests.AuthServiceTests.Utils
{
    public class AuthRepoOptions
    {
        public AuthRepositoryResult AddTokensResult { get; set; }

        public AuthRepositoryResult GetUserByNameResult { get; set; }

        public User User { get; set; } = new User
        {
            Id = 1,
            IsActive = true,
            Login = "Login",
            Password = "Password",
        };

        public TokenModel TokenModel { get; set; } =  new TokenModel(
            ("AccessToken", DateTime.Now, "accessJti"),
        ("RefreshToken", DateTime.UtcNow, "RefreshJti"));

        public static AuthRepoOptionsBuilder Create()
        {
            return new AuthRepoOptionsBuilder();
        }
    }

    public class AuthRepoOptionsBuilder
    {
        private readonly AuthRepoOptions authRepoOptions;

        public AuthRepoOptionsBuilder()
        {
            authRepoOptions = new AuthRepoOptions();
        }

        public AuthRepoOptionsBuilder AddTokensMethodReturns(AuthRepositoryResult result)
        {
            authRepoOptions.AddTokensResult = result;
            return this;
        }

        public AuthRepoOptionsBuilder GetUserByNameReturns(AuthRepositoryResult result)
        {
            authRepoOptions.GetUserByNameResult = result;
            return this;
        }

        public AuthRepoOptionsBuilder UserIsActive(bool isActive)
        {
            authRepoOptions.User.IsActive = isActive;
            return this;
        }

        public AuthRepoOptions Build()
        {
            return authRepoOptions;
        }
    }

    public static class FakeAuthRepositoryFactory
    {
        public static IAuthRepository FakeAuthRepository(AuthRepoOptions options)
        {
            var authRepo = new Mock<IAuthRepository>();

            authRepo.Setup(c => c.GetUserByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<AuthRepositoryResult, User>(options.GetUserByNameResult, options.User));

            authRepo.Setup(c =>
                    c.AddTokensAsync(It.IsAny<long>(), It.IsAny<TokenModel>(),It.IsAny<string>() ,It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<AuthRepositoryResult>(options.AddTokensResult));

            return authRepo.Object;

        }
    }
}
