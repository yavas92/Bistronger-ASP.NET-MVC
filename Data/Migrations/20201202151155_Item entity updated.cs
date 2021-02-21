using Microsoft.EntityFrameworkCore.Migrations;

namespace Bistronger.Data.Migrations
{
    public partial class Itementityupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Items_OwnerID",
                table: "Items",
                column: "OwnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_OwnerID",
                table: "Items",
                column: "OwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_OwnerID",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_OwnerID",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Items");

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
                onDelete: ReferentialAction.Cascade);
        }
    }
}