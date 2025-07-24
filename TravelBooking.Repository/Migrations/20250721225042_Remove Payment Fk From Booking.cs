using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemovePaymentFkFromBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "Bookings",
                type: "int",
                nullable: true);
        }
    }
}
