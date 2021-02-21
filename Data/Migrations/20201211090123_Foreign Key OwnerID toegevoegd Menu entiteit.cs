using Microsoft.EntityFrameworkCore.Migrations;

namespace Bistronger.Data.Migrations
{
    public partial class ForeignKeyOwnerIDtoegevoegdMenuentiteit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "Menus",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_OwnerID",
                table: "Menus",
                column: "OwnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_AspNetUsers_OwnerID",
                table: "Menus",
                column: "OwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AspNetUsers_OwnerID",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_OwnerID",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Menus");
        }
    }
}
