using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFlightModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_FlightCompanies_FlightId",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "Flights",
                newName: "FlightCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Flights_FlightId",
                table: "Flights",
                newName: "IX_Flights_FlightCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_FlightCompanies_FlightCompanyId",
                table: "Flights",
                column: "FlightCompanyId",
                principalTable: "FlightCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_FlightCompanies_FlightCompanyId",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "FlightCompanyId",
                table: "Flights",
                newName: "FlightId");

            migrationBuilder.RenameIndex(
                name: "IX_Flights_FlightCompanyId",
                table: "Flights",
                newName: "IX_Flights_FlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_FlightCompanies_FlightId",
                table: "Flights",
                column: "FlightId",
                principalTable: "FlightCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
