using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnesApi.Migrations
{
    /// <inheritdoc />
    public partial class FixBookingDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "booking_date",
                table: "Bookings",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "booking_date",
                table: "Bookings",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
