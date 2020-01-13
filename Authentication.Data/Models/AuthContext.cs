using System;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Authentication.Data.Models
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<AccessTokenEntity> AccessTokens { get; set; }

        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

    }
}
