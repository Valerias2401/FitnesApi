using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FitnesApi.Migrations
{
    /// <inheritdoc />
    public partial class AddEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SportEvents",
                columns: table => new
                {
                    id_event = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    event_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    id_court = table.Column<int>(type: "integer", nullable: false),
                    max_participants = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportEvents", x => x.id_event);
                    table.ForeignKey(
                        name: "FK_SportEvents_Courts_id_court",
                        column: x => x.id_court,
                        principalTable: "Courts",
                        principalColumn: "id_court",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventParticipants",
                columns: table => new
                {
                    id_participant = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_event = table.Column<int>(type: "integer", nullable: false),
                    id_user = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParticipants", x => x.id_participant);
                    table.ForeignKey(
                        name: "FK_EventParticipants_SportEvents_id_event",
                        column: x => x.id_event,
                        principalTable: "SportEvents",
                        principalColumn: "id_event",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventParticipants_Users_id_user",
                        column: x => x.id_user,
                        principalTable: "Users",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventResults",
                columns: table => new
                {
                    id_result = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_event = table.Column<int>(type: "integer", nullable: false),
                    result_text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventResults", x => x.id_result);
                    table.ForeignKey(
                        name: "FK_EventResults_SportEvents_id_event",
                        column: x => x.id_event,
                        principalTable: "SportEvents",
                        principalColumn: "id_event",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_id_event",
                table: "EventParticipants",
                column: "id_event");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_id_user",
                table: "EventParticipants",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_EventResults_id_event",
                table: "EventResults",
                column: "id_event");

            migrationBuilder.CreateIndex(
                name: "IX_SportEvents_id_court",
                table: "SportEvents",
                column: "id_court");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventParticipants");

            migrationBuilder.DropTable(
                name: "EventResults");

            migrationBuilder.DropTable(
                name: "SportEvents");
        }
    }
}
