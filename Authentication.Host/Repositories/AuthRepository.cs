﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models;
using Authentication.Data.Models.Domain;
using Authentication.Data.Models.Domain.Translators;
using Authentication.Data.Models.Entities;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AuthContext _context;
        private readonly ILogger _logger;
        public AuthRepository(AuthContext context, ILogger<AuthRepository> logger = null)
        {
            _context = context;
            _logger = logger ?? new NullLogger<AuthRepository>();
        }

        public async Task<Result<AuthRepositoryResult>> AddTokensAsync(long userId, TokenModel tokenModel, CancellationToken cancelationToken)
        {
            try
            {
                var refreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(c => c.Jti == tokenModel.RefreshToken.Jti, cancelationToken);

                if (refreshToken != null)
                {
                    var accessTokenEntityWithoutRefresh = new AccessTokenEntity
                    {
                        Created = DateTime.Now,
                        Exprired = tokenModel.AccessToken.Expiration,
                        Token = tokenModel.AccessToken.Value,
                        UserId = userId,
                        RefreshToken = refreshToken
                    };

                    await _context.AccessTokens.AddAsync(accessTokenEntityWithoutRefresh, cancelationToken);
                }
                else
                {
                    var refreshTokenEntity = new RefreshTokenEntity
                    {
                        Token = tokenModel.RefreshToken.Value,
                        Created = DateTime.UtcNow,
                        Expired = tokenModel.RefreshToken.Expiration,
                        Jti = tokenModel.RefreshToken.Jti,
                        IsBlocked = false,
                        UserId = userId,
                    };


                    var accessTokenEntity = new AccessTokenEntity
                    {
                        Created = DateTime.UtcNow,
                        Exprired = tokenModel.AccessToken.Expiration,
                        Token = tokenModel.AccessToken.Value,
                        UserId = userId,
                        RefreshToken = refreshTokenEntity
                    };

                    await _context.RefreshTokens.AddAsync(refreshTokenEntity, cancelationToken);
                    await _context.AccessTokens.AddAsync(accessTokenEntity, cancelationToken);
                }

                await _context.SaveChangesAsync(cancelationToken);
                return new Result<AuthRepositoryResult>(AuthRepositoryResult.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<AuthRepositoryResult>(AuthRepositoryResult.Error);
            }
        }

        public async Task<Result<AuthRepositoryResult, User>> GetUserByNameAsync(string userName, CancellationToken cancelationToken)
        {
            UserEntity userEntity;
            try
            {
                userEntity = await _context.Users
                    .AsNoTracking()
                    //.Where(x => x.IsActive)
                    .Include(p => p.Roles)
                    .ThenInclude(p => p.RoleEn)
                    .SingleOrDefaultAsync(obj => obj.Login == userName, cancelationToken);

                if (userEntity == null)
                    return new Result<AuthRepositoryResult, User>(AuthRepositoryResult.UserNotFound);

                return new Result<AuthRepositoryResult, User>(AuthRepositoryResult.Ok, userEntity.ToDomain());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<AuthRepositoryResult, User>(AuthRepositoryResult.Error, message: ex.Message);
            }
        }
    }
}
