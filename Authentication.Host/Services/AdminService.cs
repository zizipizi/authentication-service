﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data;
using Authentication.Data.Exceptions;
using Authentication.Data.Models.Domain;
using Authentication.Host.Models;
using Authentication.Host.Repositories;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using NSV.Security.JWT;
using NSV.Security.Password;

namespace Authentication.Host.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public AdminService(IUserRepository userRepository, IPasswordService passwordService, IJwtService @object)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<Result<AdminResult>> BlockUserAsync(int id, CancellationToken token)
        {
            try
            {
                await _userRepository.BlockUserAsync(id, token);
                return new Result<AdminResult>(AdminResult.Ok, $"User with id {id} blocked");
            }
            catch (EntityNotFoundException)
            {
                return new Result<AdminResult>(AdminResult.UserNotFound, $"User with id {id} not found");
            }
            catch (Exception)
            {
                return new Result<AdminResult>(AdminResult.Error, "DB error");
            }
        }

        public async Task<Result<AdminResult>> CreateUserAsync(UserCreateModel model, CancellationToken token)
        {
            try
            {
                var pass = _passwordService.Hash(model.Password);

                await _userRepository.CreateUserAsync(new User()
                {
                    Login = model.Login,
                    Password = pass.Hash,
                    Role = model.Role
                }, token);

                return new Result<AdminResult>(AdminResult.Ok, "User created");
            }
            catch (EntityNotFoundException)
            {
                return new Result<AdminResult>(AdminResult.UserExist, "User with same login exist");
            }
            catch (Exception)
            {
                return new Result<AdminResult>(AdminResult.Error, "DB error");
            }
        }

        public async Task<Result<AdminResult>> DeleteUserAsync(int id, CancellationToken token)
        {
            try
            {
                await _userRepository.DeleteUserAsync(id, token);
                return new Result<AdminResult>(AdminResult.Ok, $"User with id {id} deleted");
            }
            catch (EntityNotFoundException)
            {
                return new Result<AdminResult>(AdminResult.UserNotFound, $"User with id {id} not found");
            }
            catch (Exception)
            {
                return new Result<AdminResult>(AdminResult.Error, "DB error");
            }
        }
    }
}
