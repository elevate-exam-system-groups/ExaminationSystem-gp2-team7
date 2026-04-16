using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixEditRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AuthAppUser_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "AuthAppUser");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.CreateTable(
                name: "AuthAppUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthAppUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthAppUser_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AuthAppUser_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "AuthAppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
