using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bistronger.Data.Migrations
{
    public partial class DisplayPicturepropertytoegevoegdaanItemklasse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "DisplayPicture",
                table: "Items",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayPicture",
                table: "Items");
        }
    }
}
