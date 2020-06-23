using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Authentication.Data.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_name = table.Column<string>(maxLength: 128, nullable: false),
                    login = table.Column<string>(maxLength: 128, nullable: false),
                    password = table.Column<string>(maxLength: 1024, nullable: false),
                    created = table.Column<DateTime>(nullable: false),
                    active = table.Column<bool>(nullable: false),
                    deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Refresh_token",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(nullable: false),
                    token = table.Column<string>(nullable: true),
                    expired = table.Column<DateTime>(nullable: false),
                    created = table.Column<DateTime>(nullable: false),
                    is_blocked = table.Column<bool>(nullable: false),
                    token_jti = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refresh_token", x => x.id);
                    table.UniqueConstraint("AK_Refresh_token_token_jti", x => x.token_jti);
                    table.ForeignKey(
                        name: "FK_Refresh_token_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    role_id = table.Column<long>(nullable: false),
                    user_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.role_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_role_id",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Access_token",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(nullable: false),
                    token = table.Column<string>(nullable: true),
                    expired = table.Column<DateTime>(nullable: false),
                    created = table.Column<DateTime>(nullable: false),
                    ip_adress = table.Column<string>(nullable: true),
                    token_jti = table.Column<string>(nullable: true),
                    RefreshTokenJti = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Access_token", x => x.id);
                    table.ForeignKey(
                        name: "FK_Access_token_Refresh_token_RefreshTokenJti",
                        column: x => x.RefreshTokenJti,
                        principalTable: "Refresh_token",
                        principalColumn: "token_jti",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Access_token_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "id", "description", "role" },
                values: new object[,]
                {
                    { 1L, "Admin privilegies", "Admin" },
                    { 2L, "User privilegies", "User" },
                    { 3L, "Guest privilegies", "Guest" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "id", "created", "active", "deleted", "login", "password", "user_name" },
                values: new object[] { 1L, new DateTime(2020, 3, 13, 9, 57, 4, 39, DateTimeKind.Utc).AddTicks(9620), true, false, "Admin", "8TmZ94UJv6zkZntGk0IQ0DZgyx4YzW3p|1000|CQ/LrH+yplj0rGTBGbmJihBwTMKeHXId", "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "role_id", "user_id" },
                values: new object[] { 1L, 1L });

            migrationBuilder.CreateIndex(
                name: "IX_Access_token_RefreshTokenJti",
                table: "Access_token",
                column: "RefreshTokenJti");

            migrationBuilder.CreateIndex(
                name: "IX_Access_token_user_id",
                table: "Access_token",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Refresh_token_user_id",
                table: "Refresh_token",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_login",
                table: "User",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_user_id",
                table: "UserRole",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Access_token");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Refresh_token");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
