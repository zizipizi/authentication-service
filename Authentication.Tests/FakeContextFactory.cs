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
    }
}
