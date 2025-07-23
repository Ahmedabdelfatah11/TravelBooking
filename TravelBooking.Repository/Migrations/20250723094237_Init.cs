using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HotelCompanyId = table.Column<int>(type: "int", nullable: true),
                    FlightCompanyId = table.Column<int>(type: "int", nullable: true),
                    CarRentalCompanyId = table.Column<int>(type: "int", nullable: true),
                    TourCompanyId = table.Column<int>(type: "int", nullable: true),
                    CompanyType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CarRentalCompanyId1 = table.Column<int>(type: "int", nullable: true),
                    FlightCompanyId1 = table.Column<int>(type: "int", nullable: true),
                    HotelCompanyId1 = table.Column<int>(type: "int", nullable: true),
                    TourCompanyId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favorites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_CarRentalCompanies_CarRentalCompanyId",
                        column: x => x.CarRentalCompanyId,
                        principalTable: "CarRentalCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Favorites_CarRentalCompanies_CarRentalCompanyId1",
                        column: x => x.CarRentalCompanyId1,
                        principalTable: "CarRentalCompanies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favorites_FlightCompanies_FlightCompanyId",
                        column: x => x.FlightCompanyId,
                        principalTable: "FlightCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Favorites_FlightCompanies_FlightCompanyId1",
                        column: x => x.FlightCompanyId1,
                        principalTable: "FlightCompanies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favorites_HotelCompanies_HotelCompanyId",
                        column: x => x.HotelCompanyId,
                        principalTable: "HotelCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Favorites_HotelCompanies_HotelCompanyId1",
                        column: x => x.HotelCompanyId1,
                        principalTable: "HotelCompanies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favorites_TourCompany_TourCompanyId",
                        column: x => x.TourCompanyId,
                        principalTable: "TourCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Favorites_TourCompany_TourCompanyId1",
                        column: x => x.TourCompanyId1,
                        principalTable: "TourCompany",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HotelCompanyId = table.Column<int>(type: "int", nullable: true),
                    FlightCompanyId = table.Column<int>(type: "int", nullable: true),
                    CarRentalCompanyId = table.Column<int>(type: "int", nullable: true),
                    TourCompanyId = table.Column<int>(type: "int", nullable: true),
                    CompanyType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CarRentalCompanyId1 = table.Column<int>(type: "int", nullable: true),
                    FlightCompanyId1 = table.Column<int>(type: "int", nullable: true),
                    HotelCompanyId1 = table.Column<int>(type: "int", nullable: true),
                    TourCompanyId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_CarRentalCompanies_CarRentalCompanyId",
                        column: x => x.CarRentalCompanyId,
                        principalTable: "CarRentalCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Reviews_CarRentalCompanies_CarRentalCompanyId1",
                        column: x => x.CarRentalCompanyId1,
                        principalTable: "CarRentalCompanies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_FlightCompanies_FlightCompanyId",
                        column: x => x.FlightCompanyId,
                        principalTable: "FlightCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Reviews_FlightCompanies_FlightCompanyId1",
                        column: x => x.FlightCompanyId1,
                        principalTable: "FlightCompanies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_HotelCompanies_HotelCompanyId",
                        column: x => x.HotelCompanyId,
                        principalTable: "HotelCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Reviews_HotelCompanies_HotelCompanyId1",
                        column: x => x.HotelCompanyId1,
                        principalTable: "HotelCompanies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_TourCompany_TourCompanyId",
                        column: x => x.TourCompanyId,
                        principalTable: "TourCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Reviews_TourCompany_TourCompanyId1",
                        column: x => x.TourCompanyId1,
                        principalTable: "TourCompany",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ApplicationUserId",
                table: "Favorites",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_CarRentalCompanyId",
                table: "Favorites",
                column: "CarRentalCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_CarRentalCompanyId1",
                table: "Favorites",
                column: "CarRentalCompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_FlightCompanyId",
                table: "Favorites",
                column: "FlightCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_FlightCompanyId1",
                table: "Favorites",
                column: "FlightCompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_HotelCompanyId",
                table: "Favorites",
                column: "HotelCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_HotelCompanyId1",
                table: "Favorites",
                column: "HotelCompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_TourCompanyId",
                table: "Favorites",
                column: "TourCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_TourCompanyId1",
                table: "Favorites",
                column: "TourCompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId_HotelCompanyId_FlightCompanyId_CarRentalCompanyId_TourCompanyId",
                table: "Favorites",
                columns: new[] { "UserId", "HotelCompanyId", "FlightCompanyId", "CarRentalCompanyId", "TourCompanyId" },
                unique: true,
                filter: "[HotelCompanyId] IS NOT NULL AND [FlightCompanyId] IS NOT NULL AND [CarRentalCompanyId] IS NOT NULL AND [TourCompanyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ApplicationUserId",
                table: "Reviews",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CarRentalCompanyId",
                table: "Reviews",
                column: "CarRentalCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CarRentalCompanyId1",
                table: "Reviews",
                column: "CarRentalCompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_FlightCompanyId",
                table: "Reviews",
                column: "FlightCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_FlightCompanyId1",
                table: "Reviews",
                column: "FlightCompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_HotelCompanyId",
                table: "Reviews",
                column: "HotelCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_HotelCompanyId1",
                table: "Reviews",
                column: "HotelCompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_TourCompanyId",
                table: "Reviews",
                column: "TourCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_TourCompanyId1",
                table: "Reviews",
                column: "TourCompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId_HotelCompanyId_FlightCompanyId_CarRentalCompanyId_TourCompanyId",
                table: "Reviews",
                columns: new[] { "UserId", "HotelCompanyId", "FlightCompanyId", "CarRentalCompanyId", "TourCompanyId" },
                unique: true,
                filter: "[HotelCompanyId] IS NOT NULL AND [FlightCompanyId] IS NOT NULL AND [CarRentalCompanyId] IS NOT NULL AND [TourCompanyId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
