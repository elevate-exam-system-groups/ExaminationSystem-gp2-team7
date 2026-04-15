using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExaminationSystem.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Diplomas",
                keyColumn: "Id",
                keyValue: new Guid("d1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Diplomas",
                keyColumn: "Id",
                keyValue: new Guid("d2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Diplomas",
                keyColumn: "Id",
                keyValue: new Guid("d3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111111"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Email", "EmailConfirmed", "EmailOtpCode", "EmailOtpExpiresAt", "EmailOtpLockedUntil", "EmailVerifiedAt", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpResendCount", "OtpResendWindowStart", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImageUrl", "ResetToken", "ResetTokenExpiresAt", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UpdatedBy", "UserName" },
                values: new object[] { new Guid("a1111111-1111-1111-1111-111111111111"), 0, "ff51b962-b479-4a4e-b57e-7234ff63760e", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "admin@exam.com", true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System Admin", false, null, "ADMIN@EXAM.COM", "ADMIN@EXAM.COM", 0, null, null, null, false, null, null, null, "STATIC-SECURITY-STAMP-FOR-SEED-ADMIN", false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "admin@exam.com" });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Status" },
                values: new object[] { new Guid("a1111111-1111-1111-1111-111111111111"), "Active" });

            migrationBuilder.InsertData(
                table: "Diplomas",
                columns: new[] { "Id", "AdminId", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Description", "Status", "Title", "TotalQuizCount", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("d1111111-1111-1111-1111-111111111111"), new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Master front-end and back-end technologies including HTML, CSS, JavaScript, C#, and ASP.NET Core.", "Published", "Full-Stack Web Development", 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("d2222222-2222-2222-2222-222222222222"), new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Learn Python, statistics, data analysis, and ML algorithms from scratch to advanced level.", "Published", "Data Science & Machine Learning", 0, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("d3333333-3333-3333-3333-333333333333"), new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Build cross-platform mobile applications using Flutter and Dart.", "Draft", "Mobile App Development", 0, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null }
                });
        }
    }
}
