﻿using Authentication.Data.Models.Entities;
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

            modelBuilder.Entity<UserRolesEntity>().HasKey(k => new { k.RoleId, k.UserId });
        }
    }
}
