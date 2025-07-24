using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class BookingAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tours_TourCompany_TourCompanyId",
                table: "Tours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TourCompany",
                table: "TourCompany");

            migrationBuilder.RenameTable(
                name: "TourCompany",
                newName: "TourCompanies");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Bookings",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "RefId",
                table: "Bookings",
                newName: "TourId");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TourCompanies",
                table: "TourCompanies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CarId",
                table: "Bookings",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FlightId",
                table: "Bookings",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TourId",
                table: "Bookings",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Cars_CarId",
                table: "Bookings",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Flights_FlightId",
                table: "Bookings",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Tours_TourId",
                table: "Bookings",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_TourCompanies_TourCompanyId",
                table: "Tours",
                column: "TourCompanyId",
                principalTable: "TourCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Cars_CarId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Flights_FlightId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Tours_TourId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Tours_TourCompanies_TourCompanyId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CarId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_FlightId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_TourId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TourCompanies",
                table: "TourCompanies");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "TourCompanies",
                newName: "TourCompany");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Bookings",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "TourId",
                table: "Bookings",
                newName: "RefId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TourCompany",
                table: "TourCompany",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_TourCompany_TourCompanyId",
                table: "Tours",
                column: "TourCompanyId",
                principalTable: "TourCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
