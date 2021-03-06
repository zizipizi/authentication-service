﻿// <auto-generated />
using System;
using Authentication.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Authentication.Data.Migrations
{
    [DbContext(typeof(AuthContext))]
    partial class AuthContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Authentication.Data.Models.Entities.AccessTokenEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnName("created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Exprired")
                        .HasColumnName("expired")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("IpAdress")
                        .HasColumnName("ip_adress")
                        .HasColumnType("text");

                    b.Property<string>("Jti")
                        .HasColumnName("token_jti")
                        .HasColumnType("text");

                    b.Property<string>("RefreshTokenJti")
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .HasColumnName("token")
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RefreshTokenJti");

                    b.HasIndex("UserId");

                    b.ToTable("Access_token");
                });

            modelBuilder.Entity("Authentication.Data.Models.Entities.RefreshTokenEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnName("created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Expired")
                        .HasColumnName("expired")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsBlocked")
                        .HasColumnName("is_blocked")
                        .HasColumnType("boolean");

                    b.Property<string>("Jti")
                        .IsRequired()
                        .HasColumnName("token_jti")
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .HasColumnName("token")
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Refresh_token");
                });

            modelBuilder.Entity("Authentication.Data.Models.Entities.RoleEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .HasColumnName("role")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Description = "Admin privilegies",
                            Role = "Admin"
                        },
                        new
                        {
                            Id = 2L,
                            Description = "User privilegies",
                            Role = "User"
                        },
                        new
                        {
                            Id = 3L,
                            Description = "Guest privilegies",
                            Role = "Guest"
                        });
                });

            modelBuilder.Entity("Authentication.Data.Models.Entities.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnName("created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnName("active")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnName("deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnName("login")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password")
                        .HasColumnType("character varying(1024)")
                        .HasMaxLength(1024);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnName("user_name")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Created = new DateTime(2020, 3, 13, 9, 57, 4, 39, DateTimeKind.Utc).AddTicks(9620),
                            IsActive = true,
                            IsDeleted = false,
                            Login = "Admin",
                            Password = "8TmZ94UJv6zkZntGk0IQ0DZgyx4YzW3p|1000|CQ/LrH+yplj0rGTBGbmJihBwTMKeHXId",
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("Authentication.Data.Models.Entities.UserRolesEntity", b =>
                {
                    b.Property<long>("RoleId")
                        .HasColumnName("role_id")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("bigint");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");

                    b.HasData(
                        new
                        {
                            RoleId = 1L,
                            UserId = 1L
                        });
                });

            modelBuilder.Entity("Authentication.Data.Models.Entities.AccessTokenEntity", b =>
                {
                    b.HasOne("Authentication.Data.Models.Entities.RefreshTokenEntity", "RefreshToken")
                        .WithMany("AccessToken")
                        .HasForeignKey("RefreshTokenJti")
                        .HasPrincipalKey("Jti");

                    b.HasOne("Authentication.Data.Models.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Authentication.Data.Models.Entities.RefreshTokenEntity", b =>
                {
                    b.HasOne("Authentication.Data.Models.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Authentication.Data.Models.Entities.UserRolesEntity", b =>
                {
                    b.HasOne("Authentication.Data.Models.Entities.RoleEntity", "RoleEn")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Authentication.Data.Models.Entities.UserEntity", "UserEn")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
