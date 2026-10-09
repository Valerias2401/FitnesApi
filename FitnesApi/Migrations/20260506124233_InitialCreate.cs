using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FitnesApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abonements",
                columns: table => new
                {
                    id_abonement = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name_abonement = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    duration_days = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonements", x => x.id_abonement);
                });

            migrationBuilder.CreateTable(
                name: "Courts",
                columns: table => new
                {
                    id_court = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    sport_type = table.Column<string>(type: "text", nullable: true),
                    price_per_hour = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courts", x => x.id_court);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id_role = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name_role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id_role);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: true),
                    birth_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    id_role = table.Column<int>(type: "integer", nullable: false),
                    balance = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id_user);
                    table.ForeignKey(
                        name: "FK_Users_Roles_id_role",
                        column: x => x.id_role,
                        principalTable: "Roles",
                        principalColumn: "id_role",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    id_booking = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_user = table.Column<int>(type: "integer", nullable: false),
                    id_court = table.Column<int>(type: "integer", nullable: false),
                    booking_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    start_time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    end_time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.id_booking);
                    table.ForeignKey(
                        name: "FK_Bookings_Courts_id_court",
                        column: x => x.id_court,
                        principalTable: "Courts",
                        principalColumn: "id_court",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_id_user",
                        column: x => x.id_user,
                        principalTable: "Users",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    id_section = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    trainer_id = table.Column<int>(type: "integer", nullable: true),
                    max_participants = table.Column<int>(type: "integer", nullable: true),
                    schedule_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.id_section);
                    table.ForeignKey(
                        name: "FK_Sections_Users_trainer_id",
                        column: x => x.trainer_id,
                        principalTable: "Users",
                        principalColumn: "id_user");
                });

            migrationBuilder.CreateTable(
                name: "UserAbonements",
                columns: table => new
                {
                    id_user_abonement = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_user = table.Column<int>(type: "integer", nullable: false),
                    id_abonement = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAbonements", x => x.id_user_abonement);
                    table.ForeignKey(
                        name: "FK_UserAbonements_Abonements_id_abonement",
                        column: x => x.id_abonement,
                        principalTable: "Abonements",
                        principalColumn: "id_abonement",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAbonements_Users_id_user",
                        column: x => x.id_user,
                        principalTable: "Users",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SectionEnrollments",
                columns: table => new
                {
                    id_enrollment = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_user = table.Column<int>(type: "integer", nullable: false),
                    id_section = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionEnrollments", x => x.id_enrollment);
                    table.ForeignKey(
                        name: "FK_SectionEnrollments_Sections_id_section",
                        column: x => x.id_section,
                        principalTable: "Sections",
                        principalColumn: "id_section",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SectionEnrollments_Users_id_user",
                        column: x => x.id_user,
                        principalTable: "Users",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_id_court",
                table: "Bookings",
                column: "id_court");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_id_user",
                table: "Bookings",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_SectionEnrollments_id_section",
                table: "SectionEnrollments",
                column: "id_section");

            migrationBuilder.CreateIndex(
                name: "IX_SectionEnrollments_id_user",
                table: "SectionEnrollments",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_trainer_id",
                table: "Sections",
                column: "trainer_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserAbonements_id_abonement",
                table: "UserAbonements",
                column: "id_abonement");

            migrationBuilder.CreateIndex(
                name: "IX_UserAbonements_id_user",
                table: "UserAbonements",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_Users_id_role",
                table: "Users",
                column: "id_role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "SectionEnrollments");

            migrationBuilder.DropTable(
                name: "UserAbonements");

            migrationBuilder.DropTable(
                name: "Courts");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Abonements");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
