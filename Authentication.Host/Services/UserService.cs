﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Exceptions;
using Authentication.Host.Models;
using Authentication.Host.Models.Translators;
using Authentication.Host.Repositories;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using NSV.Security.JWT;
using NSV.Security.Password;
using Processing.Kafka.Producer;

namespace Authentication.Host.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly ICacheRepository _cacheRepository;
        private readonly IProducerFactory<int, string> _kafka;

        public UserService(IUserRepository userRepository,
            IPasswordService passwordService, 
            IJwtService jwtService, 
            ICacheRepository cacheRepository,
            IProducerFactory<int,string> kafka
            )
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _cacheRepository = cacheRepository;
            _kafka = kafka;
        }

        public async Task<Result<HttpStatusCode>> SignOutAsync(long id, string refreshJti, CancellationToken cancellationToken)
        {
            var blockResult = await _userRepository.BlockRefreshTokenAsync(refreshJti, cancellationToken);
            if (blockResult.Value == UserRepositoryResult.Error)
                return new Result<HttpStatusCode>(HttpStatusCode.BadRequest);

            var isRefreshTokenBlockedResult = await _cacheRepository.IsRefreshTokenBlockedAsync(refreshJti, cancellationToken);

            if (isRefreshTokenBlockedResult.Value == CacheRepositoryResult.IsNotBlocked)
                await _cacheRepository.AddRefreshTokenToBlacklistAsync(blockResult.Model, cancellationToken);

            using (var message = _kafka.Create("BlockedTokens"))
            {
                var res = await message.SendAsync((int)id, blockResult.Model.AccessToken.Value);
            }

            return new Result<HttpStatusCode>(HttpStatusCode.NoContent);
        }

        public async Task<Result<HttpStatusCode, BodyTokenModel>> ChangePasswordAsync(ChangePassModel model, long id, string accessToken, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(id, cancellationToken);
            if (user.Value == UserRepositoryResult.Error)
                return new Result<HttpStatusCode, BodyTokenModel>(HttpStatusCode.BadRequest);

            var passwordValidateResult = _passwordService.Validate(model.OldPassword, user.Model.Password);

            if (passwordValidateResult.Result == PasswordValidateResult.ValidateResult.Invalid)
                return new Result<HttpStatusCode, BodyTokenModel>(HttpStatusCode.BadRequest, message: "Wrong old password");

            var newPasswordHash = _passwordService.Hash(model.NewPassword);

            var updatePasswordResult = await _userRepository.UpdateUserPasswordAsync(user.Model.Id, newPasswordHash.Hash, cancellationToken);

            if (updatePasswordResult.Value == UserRepositoryResult.Error)
                return new Result<HttpStatusCode, BodyTokenModel>(HttpStatusCode.ServiceUnavailable);

            var blockAllTokensResult = await _userRepository.BlockAllRefreshTokensAsync(user.Model.Id, cancellationToken);

            if (blockAllTokensResult.Value == UserRepositoryResult.Error)
                return new Result<HttpStatusCode, BodyTokenModel>(HttpStatusCode.ServiceUnavailable);

            await _cacheRepository.AddRefreshTokensToBlacklistAsync(blockAllTokensResult.Model, cancellationToken);

            var newTokens = _jwtService.IssueAccessToken(user.Model.Id.ToString(), user.Model.Login, user.Model.Role);
            
            return new Result<HttpStatusCode, BodyTokenModel>(HttpStatusCode.OK, model: newTokens.Tokens.toBodyTokenModel());
        }
    }
}
