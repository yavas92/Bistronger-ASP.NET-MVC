using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bistronger.Data.Migrations
{
    public partial class restructuredreservationdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservedUntil",
                table: "Tables");

            migrationBuilder.RenameColumn(
                name: "ReservationDate",
                table: "Reservations",
                newName: "ReservationDateTo");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDateFrom",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationDateFrom",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "ReservationDateTo",
                table: "Reservations",
                newName: "ReservationDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservedUntil",
                table: "Tables",
                type: "datetime2",
                nullable: true);
        }
    }
}
