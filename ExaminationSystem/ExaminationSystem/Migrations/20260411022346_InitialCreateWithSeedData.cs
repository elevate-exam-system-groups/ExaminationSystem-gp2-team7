using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "EmailOtpCode", "EmailOtpExpiresAt", "EmailOtpLockedUntil", "EmailVerifiedAt", "FailedLoginAttempts", "FullName", "LockoutEnabled", "LockoutEnd", "LockoutUntil", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImageUrl", "ResetToken", "ResetTokenExpiresAt", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName", "UserType" },
                values: new object[] { new Guid("a1111111-1111-1111-1111-111111111111"), 0, "c6c5e5ba-54f0-42f8-9df7-2e8a50dd0d00", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@exam.com", true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, "System Admin", false, null, null, "ADMIN@EXAM.COM", "ADMIN@EXAM.COM", null, null, false, null, null, null, "STATIC-SECURITY-STAMP-FOR-SEED-ADMIN", false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@exam.com", "Admin" });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "DeletedAt" },
                values: new object[] { new Guid("a1111111-1111-1111-1111-111111111111"), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111111"));
        }
    }
}
