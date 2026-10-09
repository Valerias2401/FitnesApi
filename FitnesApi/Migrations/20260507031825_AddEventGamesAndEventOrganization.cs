using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FitnesApi.Migrations
{
    /// <inheritdoc />
    public partial class AddEventGamesAndEventOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "bracket_info",
                table: "SportEvents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "game_schedule",
                table: "SportEvents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "photo_gallery",
                table: "SportEvents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "payment_amount",
                table: "Bookings",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "qr_code",
                table: "Bookings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "visit_limit",
                table: "Abonements",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EventGames",
                columns: table => new
                {
                    id_game = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_event = table.Column<int>(type: "integer", nullable: false),
                    first_participant = table.Column<string>(type: "text", nullable: false),
                    second_participant = table.Column<string>(type: "text", nullable: false),
                    game_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    result = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventGames", x => x.id_game);
                    table.ForeignKey(
                        name: "FK_EventGames_SportEvents_id_event",
                        column: x => x.id_event,
                        principalTable: "SportEvents",
                        principalColumn: "id_event",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SectionAttendances",
                columns: table => new
                {
                    id_attendance = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_enrollment = table.Column<int>(type: "integer", nullable: false),
                    attendance_date = table.Column<DateTime>(type: "date", nullable: false),
                    is_present = table.Column<bool>(type: "boolean", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionAttendances", x => x.id_attendance);
                    table.ForeignKey(
                        name: "FK_SectionAttendances_SectionEnrollments_id_enrollment",
                        column: x => x.id_enrollment,
                        principalTable: "SectionEnrollments",
                        principalColumn: "id_enrollment",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventGames_id_event",
                table: "EventGames",
                column: "id_event");

            migrationBuilder.CreateIndex(
                name: "IX_SectionAttendances_id_enrollment",
                table: "SectionAttendances",
                column: "id_enrollment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventGames");

            migrationBuilder.DropTable(
                name: "SectionAttendances");

            migrationBuilder.DropColumn(
                name: "bracket_info",
                table: "SportEvents");

            migrationBuilder.DropColumn(
                name: "game_schedule",
                table: "SportEvents");

            migrationBuilder.DropColumn(
                name: "photo_gallery",
                table: "SportEvents");

            migrationBuilder.DropColumn(
                name: "payment_amount",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "qr_code",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "visit_limit",
                table: "Abonements");
        }
    }
}
