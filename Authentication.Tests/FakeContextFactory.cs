using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Authentication.Data.Models;
using Authentication.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;

namespace Authentication.Tests
{
    public static class FakeContextFactory
    {
        #region GetUserById

        public static AuthContext GetUserById_Ok()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "Get_User_By_Id")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = 1
            };

            context.Add(user);
            context.SaveChanges();

            return context;
        }

        public static AuthContext GetUserById_EntityException()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "Get_User_By_Id_Entity_Exception")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = 1
            };

            context.Add(user);
            context.SaveChanges();

            return context;
        }

        #endregion


        #region GetUserByName
        public static AuthContext GetUserByName_Ok()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "Get_User_By_Name")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = 1
            };

            var user2 = new UserEntity
            {
                Login = "Login2",
                Password = "Password2",
                Id = 2
            };

            context.Add(user);
            context.Add(user2);
            context.SaveChanges();

            return context;
        }


        #endregion


        #region CreateUser

        public static AuthContext CreateUser_Ok()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "CreateUser_Ok")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        public static AuthContext CreateUser_EntityException()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "CreateUser_Exception")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }
        #endregion


        #region BlockUser
        public static AuthContext BlockUser_Ok()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "BlockUser_Ok")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = 1,
                IsActive = true
            };

            context.Users.Add(user);
            context.SaveChanges();

            return context;
        }

        public static AuthContext BlockUser_EntityException()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "BlockUser_Ok")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = 1,
                IsActive = true
            };

            context.Users.Add(user);
            context.SaveChanges();

            return context;
        }
        #endregion


        #region UpdateUserPassword

        public static AuthContext UpdateUserPassword_Ok()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "UpdateUserPassword_Ok")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = 1,
                IsActive = true
            };

            context.Users.Add(user);
            context.SaveChanges();

            return context;
        }

        public static AuthContext UpdateUserPassword_EntityException()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "UpdateUserPassword_EntityException")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = 1,
                IsActive = true
            };

            context.Users.Add(user);
            context.SaveChanges();

            return context;
        }

        #endregion

        #region CheckRefreshTokenForBlock

        public static AuthContext CheckRefreshToken_Ok()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "CheckRefreshToken_Ok")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var refreshToken = new RefreshTokenEntity
            {
                Created = DateTime.Now,
                Expired = DateTime.Now.AddMinutes(15),
                Id = 1,
                IsBlocked = false,
                Jti = "1234567890-0987654321-1234567890",
                Token = "laksddfkjsdfjsdkfljsdkfjwioef",
            };

            context.RefreshTokens.Add(refreshToken);
            context.SaveChanges();

            return context;
        }

        public static AuthContext CheckRefreshToken_Blocked()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "CheckRefreshToken_Blocked")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var refreshToken = new RefreshTokenEntity
            {
                Created = DateTime.Now,
                Expired = DateTime.Now.AddMinutes(15),
                Id = 1,
                IsBlocked = true,
                Jti = "1234567890-0987654321-1234567890",
                Token = "laksddfkjsdfjsdkfljsdkfjwioef",
            };

            context.RefreshTokens.Add(refreshToken);
            context.SaveChanges();

            return context;
        }

        #endregion


        #region BlockAllTokens

        public static AuthContext BlockAllTokens_Ok()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "BlockAllTokens_Ok")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = 1,
                IsActive = true
            };

            var refreshToken = new RefreshTokenEntity
            {
                Created = DateTime.Now,
                Expired = DateTime.Now.AddMinutes(15),
                Id = 1,
                IsBlocked = false,
                Jti = "1234567890-0987654321-1234567890",
                Token = "laksddfkjsdfjsdkfljsdkfjwioef",
                User = user
            };

            var refreshToken2 = new RefreshTokenEntity
            {
                Created = DateTime.Now,
                Expired = DateTime.Now.AddMinutes(15),
                Id = 2,
                IsBlocked = false,
                Jti = "1234123567890-098765432321-123456734890",
                Token = "laksddfkjsdfjsdkflasdasdqwjsdkfjwioef",
                User = user
            };

            context.Users.Add(user);
            context.RefreshTokens.Add(refreshToken);
            context.RefreshTokens.Add(refreshToken2);
            context.SaveChanges();

            return context;
        }

        #endregion

        #region AddTokens

        public static AuthContext AddTokensWithRefresh()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "AddTokensWithRefresh")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = 1,
                IsActive = true
            };

            context.Users.Add(user);
            context.SaveChanges();
            return context;
        }

        public static AuthContext AddTokenWithoutRefresh()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "AddTokensWithoutRefresh")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = 1,
                IsActive = true
            };

            context.Users.Add(user);
            context.SaveChanges();
            return context;
        }

        

        #endregion
    }
}
