using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addadmins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "TourCompanies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "HotelCompanies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "FlightCompanies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "CarRentalCompanies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CarRentalCompanies_AspNetUsers_AdminId",
                table: "CarRentalCompanies",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightCompanies_AspNetUsers_AdminId",
                table: "FlightCompanies",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelCompanies_AspNetUsers_AdminId",
                table: "HotelCompanies",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TourCompanies_AspNetUsers_AdminId",
                table: "TourCompanies",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarRentalCompanies_AspNetUsers_AdminId",
                table: "CarRentalCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightCompanies_AspNetUsers_AdminId",
                table: "FlightCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelCompanies_AspNetUsers_AdminId",
                table: "HotelCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_TourCompanies_AspNetUsers_AdminId",
                table: "TourCompanies");

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

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "TourCompanies");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "HotelCompanies");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "FlightCompanies");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "CarRentalCompanies");
        }
    }
}
