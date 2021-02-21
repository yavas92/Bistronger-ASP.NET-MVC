using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Bistronger.Data.Migrations
{
    public partial class reserveringv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessHours_Reservations_ReservationID",
                table: "BusinessHours");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Businesses_BusinessToReserveID",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Orders_OrderID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_BusinessToReserveID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_OrderID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_BusinessHours_ReservationID",
                table: "BusinessHours");

            migrationBuilder.DropColumn(
                name: "BusinessID",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "BusinessToReserveID",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ReservationID",
                table: "BusinessHours");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Reservations",
                newName: "TableID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TableID",
                table: "Reservations",
                column: "TableID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Tables_TableID",
                table: "Reservations",
                column: "TableID",
                principalTable: "Tables",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Reservations",
                nullable: true,
                defaultValueSql: "GetDate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Tables_TableID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_TableID",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "TableID",
                table: "Reservations",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "BusinessID",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BusinessToReserveID",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderID",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderType",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReservationID",
                table: "BusinessHours",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_BusinessToReserveID",
                table: "Reservations",
                column: "BusinessToReserveID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_OrderID",
                table: "Reservations",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessHours_ReservationID",
                table: "BusinessHours",
                column: "ReservationID");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessHours_Reservations_ReservationID",
                table: "BusinessHours",
                column: "ReservationID",
                principalTable: "Reservations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Businesses_BusinessToReserveID",
                table: "Reservations",
                column: "BusinessToReserveID",
                principalTable: "Businesses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Orders_OrderID",
                table: "Reservations",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Reservations",
                oldNullable: false,
                oldDefaultValueSql: null); ;
        }
    }
}