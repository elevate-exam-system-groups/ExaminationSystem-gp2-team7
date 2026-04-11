using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExaminationSystem.Migrations
{
    /// <inheritdoc />
    public partial class seedingDiplomas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111111"),
                column: "ConcurrencyStamp",
                value: "ff51b962-b479-4a4e-b57e-7234ff63760e");

            migrationBuilder.InsertData(
                table: "Diplomas",
                columns: new[] { "Id", "AdminId", "CreatedAt", "DeletedAt", "Description", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("d1111111-1111-1111-1111-111111111111"), new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Master front-end and back-end technologies including HTML, CSS, JavaScript, C#, and ASP.NET Core.", "Published", "Full-Stack Web Development", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d2222222-2222-2222-2222-222222222222"), new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, "Learn Python, statistics, data analysis, and ML algorithms from scratch to advanced level.", "Published", "Data Science & Machine Learning", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Diplomas",
                columns: new[] { "Id", "AdminId", "CreatedAt", "DeletedAt", "Description", "Title", "UpdatedAt" },
                values: new object[] { new Guid("d3333333-3333-3333-3333-333333333333"), new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Build cross-platform mobile applications using Flutter and Dart.", "Mobile App Development", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111111"),
                column: "ConcurrencyStamp",
                value: "c6c5e5ba-54f0-42f8-9df7-2e8a50dd0d00");
        }
    }
}
