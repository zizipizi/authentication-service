﻿// <auto-generated />
using System;
using Authentication.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Authentication.Data.Migrations
{
    [DbContext(typeof(AuthContext))]
    partial class AuthContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("Authentication.Data.Models.Entities.AccessTokenEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Expriry")
                        .HasColumnType("TEXT");

                    b.Property<long>("RefreshId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Token")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RefreshId");

                    b.ToTable("Access_token");
                });

            modelBuilder.Entity("Authentication.Data.Models.Entities.RefreshTokenEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Expiry")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Jti")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Refresh_token");
                });

            modelBuilder.Entity("Authentication.Data.Models.Entities.RoleEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Authentication.Data.Models.Entities.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(1024);

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("Authentication.Data.Models.Entities.UserRolesEntity", b =>
                {
                    b.Property<long>("RoleId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Authentication.Data.Models.Entities.AccessTokenEntity", b =>
                {
                    b.HasOne("Authentication.Data.Models.Entities.RefreshTokenEntity", "RefreshToken")
                        .WithMany()
                        .HasForeignKey("RefreshId")
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
