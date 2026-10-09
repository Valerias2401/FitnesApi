using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnesApi.Migrations
{
    /// <inheritdoc />
    public partial class FinalSportComplexUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "condition",
                table: "InventoryItems");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "InventoryItems",
                newName: "comment");

            migrationBuilder.AlterColumn<string>(
                name: "category",
                table: "InventoryItems",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "need_repair",
                table: "InventoryItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "purchase_required",
                table: "InventoryItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "need_repair",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "purchase_required",
                table: "InventoryItems");

            migrationBuilder.RenameColumn(
                name: "comment",
                table: "InventoryItems",
                newName: "location");

            migrationBuilder.AlterColumn<string>(
                name: "category",
                table: "InventoryItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "condition",
                table: "InventoryItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
