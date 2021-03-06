﻿using System;
using System.Collections.Generic;
using Authentication.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Data.Models
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<RoleEntity> Roles { get; set; }

        public DbSet<UserRolesEntity> UsersRoles { get; set; }

        public DbSet<AccessTokenEntity> AccessTokens { get; set; }

        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasIndex(a => a.Login).IsUnique();

            modelBuilder.Entity<UserRolesEntity>().HasKey(k => new {k.RoleId, k.UserId});

            modelBuilder.Entity<AccessTokenEntity>().HasOne(c => c.RefreshToken).WithMany(c => c.AccessToken)
                .HasPrincipalKey(c => c.Jti);

            modelBuilder.Entity<RoleEntity>().HasData(new List<RoleEntity>
            {
                new RoleEntity
                {
                    Id = 1,
                    Role = "Admin",
                    Description = "Admin privilegies"
                },
                new RoleEntity
                {
                    Id = 2,
                    Role = "User",
                    Description = "User privilegies"
                },
                new RoleEntity
                {
                    Id = 3,
                    Role = "Guest",
                    Description = "Guest privilegies"
                }
            });

            modelBuilder.Entity<UserEntity>().HasData(new UserEntity
            {
                Id = 1,
                Login = "Admin",
                UserName = "admin",
                Password = "8TmZ94UJv6zkZntGk0IQ0DZgyx4YzW3p|1000|CQ/LrH+yplj0rGTBGbmJihBwTMKeHXId", //password: Admin123
                Created = DateTime.UtcNow,
                IsActive = true,
            });

            modelBuilder.Entity<UserRolesEntity>().HasData(new UserRolesEntity
            {
                RoleId = 1,
                UserId = 1
            });
        }
    }
}
