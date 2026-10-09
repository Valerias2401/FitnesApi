using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnesApi.Migrations
{
    /// <inheritdoc />
    public partial class Initialtwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "birth_date",
                table: "Users",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "birth_date",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);
        }
    }
}
