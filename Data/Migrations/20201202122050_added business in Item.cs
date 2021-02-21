using Microsoft.EntityFrameworkCore.Migrations;

namespace Bistronger.Data.Migrations
{
    public partial class addedbusinessinItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessID",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Items_BusinessID",
                table: "Items",
                column: "BusinessID");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Businesses_BusinessID",
                table: "Items",
                column: "BusinessID",
                principalTable: "Businesses",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Businesses_BusinessID",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_BusinessID",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "BusinessID",
                table: "Items");
        }
    }
}
