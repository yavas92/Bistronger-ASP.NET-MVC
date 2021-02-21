using Microsoft.EntityFrameworkCore.Migrations;

namespace Bistronger.Data.Migrations
{
    public partial class BusinessMenuaddedMenuchanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Businesses_BusinessID",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_BusinessID",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "BusinessID",
                table: "Menus");

            migrationBuilder.AddColumn<string>(
                name: "Naam",
                table: "Menus",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BusinessMenus",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuID = table.Column<int>(type: "int", nullable: false),
                    BusinessID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessMenus", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BusinessMenus_Businesses_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "Businesses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BusinessMenus_Menus_MenuID",
                        column: x => x.MenuID,
                        principalTable: "Menus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessMenus_BusinessID",
                table: "BusinessMenus",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessMenus_MenuID",
                table: "BusinessMenus",
                column: "MenuID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessMenus");

            migrationBuilder.DropColumn(
                name: "Naam",
                table: "Menus");

            migrationBuilder.AddColumn<int>(
                name: "BusinessID",
                table: "Menus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_BusinessID",
                table: "Menus",
                column: "BusinessID");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Businesses_BusinessID",
                table: "Menus",
                column: "BusinessID",
                principalTable: "Businesses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
