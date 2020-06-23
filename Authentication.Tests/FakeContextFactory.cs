using System;
using System.Linq;
using Authentication.Data.Models;
using Authentication.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Authentication.Tests
{ 
    public static class FakeContextFactory
    {
        #region GetUserById

        public static AuthContext GetUserById_Ok(out long userId)
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "Get_User_By_Id")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            userId = context.Users.Max(c => c.Id) + 1;

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = userId
            };

            context.Add(user);
            context.SaveChanges();

            return context;
        }

        public static AuthContext GetUserById_EntityException(out long userId)
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "Get_User_By_Id_Entity_Exception")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            userId = context.Users.Max(c => c.Id) + 1;

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = userId
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

            long userId = context.Users.Max(c => c.Id) + 1;

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
            };

            var user2 = new UserEntity
            {
                Login = "Login2",
                Password = "Password2",
            };

            context.Add(user);
            context.Add(user2);
            context.SaveChanges();

            return context;
        }

        public static AuthContext GetUserByName_NotFound()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "Get_User_By_Name_NotFound")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

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

        public static AuthContext CreateUser_UserExist()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "CreateUser_Exist")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }
        #endregion


        #region BlockUser
        public static AuthContext BlockUser_Ok(out long userId)
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "BlockUser_Ok")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            userId = context.Users.Max(c => c.Id) + 1;

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = userId,
                IsActive = true
            };

            context.Users.Add(user);
            context.SaveChanges();

            return context;
        }

        public static AuthContext BlockUser_UserNotFound()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "BlockUser_UserNotFound")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                IsActive = true
            };

            context.Users.Add(user);
            context.SaveChanges();

            return context;
        }

        public static AuthContext BlockUser_Exception()
        {
            //var options = new DbContextOptionsBuilder<AuthContext>()
            //    .UseInMemoryDatabase(databaseName: "BlockUser_Exception")
            //    .Options;

            //AuthContext context = new AuthContext(options);
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //var user = new UserEntity
            //{
            //    Login = "Login",
            //    Password = "Password",
            //    IsActive = true
            //};

            //context.Users.Add(user);
            //context.SaveChanges();
            var context = new Mock<AuthContext>();
            var dbSet = new Mock<DbSet<UserEntity>>();

            dbSet.As<IQueryable<UserEntity>>().Setup(c => c.GetEnumerator())
                .Throws(new Exception("asd"));

            context.Setup(c => c.Users).Returns(dbSet.Object);


            return context.Object;
        }
        #endregion

        #region UpdateUserPassword

        public static AuthContext UpdateUserPassword_Ok(out long userId)
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "UpdateUserPassword_Ok")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            userId = context.Users.Max(c => c.Id) + 1;

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = userId,
                IsActive = true
            };

            context.Users.Add(user);
            context.SaveChanges();

            return context;
        }

        public static AuthContext UpdateUserPassword_EntityException(out long userId)
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "UpdateUserPassword_EntityException")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            userId = context.Users.Max(c => c.Id) + 1;

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = userId,
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

        public static AuthContext BlockAllTokens_Ok(out long userId)
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "BlockAllTokens_Ok")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            userId = context.Users.Max(c => c.Id) + 1;

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = userId,
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

        public static AuthContext AddTokensWithRefresh(out long userId)
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "AddTokensWithRefresh")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            userId = context.Users.Max(c => c.Id) + 1;

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = userId,
                IsActive = true
            };

            context.Users.Add(user);
            context.SaveChanges();
            return context;
        }

        public static AuthContext AddTokenWithoutRefresh(out long userId)
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "AddTokensWithoutRefresh")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            userId = context.Users.Max(c => c.Id) + 1;

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                Id = userId,
                IsActive = true
            };

            context.Users.Add(user);
            context.SaveChanges();
            return context;
        }

        #endregion

        public static AuthContext BlockRefreshToken_Ok(out string refreshJti)
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase(databaseName: "Block_Refresh_Token_Ok")
                .Options;

            AuthContext context = new AuthContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new UserEntity
            {
                Login = "Login",
                Password = "Password",
                IsActive = true
            };

            context.Users.Add(user);
            context.SaveChanges();

            refreshJti = "1234567890-0987654321-1234567890";

            var refreshToken = new RefreshTokenEntity
            {
                Created = DateTime.Now,
                Expired = DateTime.Now.AddMinutes(15),
                IsBlocked = false,
                Jti = refreshJti,
                Token = "laksddfkjsdfjsdkfljsdkfjwioef",
                User = user
            };

            var accessToken = new AccessTokenEntity
            {
                Created = DateTime.Now,
                Exprired = DateTime.Now.AddMinutes(30),
                RefreshToken = refreshToken,
                Token = "asdasdasda213213",
                User = user
            };

            context.RefreshTokens.Add(refreshToken);
            context.AccessTokens.Add(accessToken);
            context.SaveChanges();

            return context;
        }
    }
}
