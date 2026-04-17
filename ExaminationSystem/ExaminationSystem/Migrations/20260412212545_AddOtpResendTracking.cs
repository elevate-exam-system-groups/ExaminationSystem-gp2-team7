using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddOtpResendTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OtpResendCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OtpResendWindowStart",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtpResendCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OtpResendWindowStart",
                table: "AspNetUsers");
        }
    }
}
