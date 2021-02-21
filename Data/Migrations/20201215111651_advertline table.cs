using Microsoft.EntityFrameworkCore.Migrations;

namespace Bistronger.Data.Migrations
{
    public partial class advertlinetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvertLines",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    AdvertID = table.Column<int>(type: "int", nullable: true),
                    OriginalShows = table.Column<int>(type: "int", nullable: false),
                    PricePaid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertLines", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AdvertLines_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertLines_OrderID",
                table: "AdvertLines",
                column: "OrderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertLines");
        }
    }
}