using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFavourite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CompanyType",
                table: "Favorites",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "TourId",
                table: "Favorites",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_TourId",
                table: "Favorites",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId_TourId",
                table: "Favorites",
                columns: new[] { "UserId", "TourId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Tours_TourId",
                table: "Favorites",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Tours_TourId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_TourId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserId_TourId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "TourId",
                table: "Favorites");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyType",
                table: "Favorites",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
