using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class v21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_FlightCompanies_FlightId",
                table: "Flights");

            migrationBuilder.AlterColumn<int>(
                name: "FlightId",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_FlightCompanies_FlightId",
                table: "Flights",
                column: "FlightId",
                principalTable: "FlightCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_FlightCompanies_FlightId",
                table: "Flights");

            migrationBuilder.AlterColumn<int>(
                name: "FlightId",
                table: "Flights",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_FlightCompanies_FlightId",
                table: "Flights",
                column: "FlightId",
                principalTable: "FlightCompanies",
                principalColumn: "Id");
        }
    }
}
