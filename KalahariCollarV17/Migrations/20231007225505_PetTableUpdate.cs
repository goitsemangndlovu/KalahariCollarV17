using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KalahariCollarV17.Migrations
{
    /// <inheritdoc />
    public partial class PetTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Pet",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Pet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrackerID",
                table: "Pet",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Pet");

            migrationBuilder.DropColumn(
                name: "TrackerID",
                table: "Pet");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Pet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
