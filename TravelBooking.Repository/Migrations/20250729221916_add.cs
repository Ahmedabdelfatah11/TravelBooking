using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TourCompanies_AdminId",
                table: "TourCompanies");

            migrationBuilder.DropIndex(
                name: "IX_HotelCompanies_AdminId",
                table: "HotelCompanies");

            migrationBuilder.DropIndex(
                name: "IX_FlightCompanies_AdminId",
                table: "FlightCompanies");

            migrationBuilder.DropIndex(
                name: "IX_CarRentalCompanies_AdminId",
                table: "CarRentalCompanies");

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "TourCompanies",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "HotelCompanies",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "FlightCompanies",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "CarRentalCompanies",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_TourCompanies_AdminId",
                table: "TourCompanies",
                column: "AdminId",
                unique: true,
                filter: "[AdminId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_HotelCompanies_AdminId",
                table: "HotelCompanies",
                column: "AdminId",
                unique: true,
                filter: "[AdminId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FlightCompanies_AdminId",
                table: "FlightCompanies",
                column: "AdminId",
                unique: true,
                filter: "[AdminId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CarRentalCompanies_AdminId",
                table: "CarRentalCompanies",
                column: "AdminId",
                unique: true,
                filter: "[AdminId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TourCompanies_AdminId",
                table: "TourCompanies");

            migrationBuilder.DropIndex(
                name: "IX_HotelCompanies_AdminId",
                table: "HotelCompanies");

            migrationBuilder.DropIndex(
                name: "IX_FlightCompanies_AdminId",
                table: "FlightCompanies");

            migrationBuilder.DropIndex(
                name: "IX_CarRentalCompanies_AdminId",
                table: "CarRentalCompanies");

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "TourCompanies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "HotelCompanies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "FlightCompanies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "CarRentalCompanies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TourCompanies_AdminId",
                table: "TourCompanies",
                column: "AdminId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelCompanies_AdminId",
                table: "HotelCompanies",
                column: "AdminId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlightCompanies_AdminId",
                table: "FlightCompanies",
                column: "AdminId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarRentalCompanies_AdminId",
                table: "CarRentalCompanies",
                column: "AdminId",
                unique: true);
        }
    }
}
