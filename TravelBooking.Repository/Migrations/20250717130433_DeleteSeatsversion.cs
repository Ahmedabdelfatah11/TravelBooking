using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class DeleteSeatsversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableBusinessSeats",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "AvailableFirstClassSeats",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "AvailableStanderedSeats",
                table: "Flights",
                newName: "AvailableSeats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvailableSeats",
                table: "Flights",
                newName: "AvailableStanderedSeats");

            migrationBuilder.AddColumn<int>(
                name: "AvailableBusinessSeats",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailableFirstClassSeats",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
