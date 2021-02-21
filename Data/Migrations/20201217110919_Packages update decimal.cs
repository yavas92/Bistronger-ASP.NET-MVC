using Microsoft.EntityFrameworkCore.Migrations;

namespace Bistronger.Data.Migrations
{
    public partial class Packagesupdatedecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Pacxkages",
                table: "Pacxkages");

            migrationBuilder.RenameTable(
                name: "Pacxkages",
                newName: "Packages");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Packages",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Packages",
                table: "Packages",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Packages",
                table: "Packages");

            migrationBuilder.RenameTable(
                name: "Packages",
                newName: "Pacxkages");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Pacxkages",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pacxkages",
                table: "Pacxkages",
                column: "ID");
        }
    }
}
