using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBooking.Repository.Migrations
{
    /// <inheritdoc />
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
    public partial class InitialCreation : Migration
========
    public partial class InitialCreate : Migration
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarRentalCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Rating = table.Column<string>(type: "nvarchar(max)", nullable: true)
========
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Rating = table.Column<string>(type: "nvarchar(max)", nullable: false)
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarRentalCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlightCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Rating = table.Column<string>(type: "nvarchar(max)", nullable: true)
========
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rating = table.Column<string>(type: "nvarchar(max)", nullable: false)
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<string>(type: "nvarchar(max)", nullable: false)
========
                    Rating = table.Column<int>(type: "int", nullable: false)
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                name: "TourCompany",
========
                name: "Tours",
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    rating = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourCompany", x => x.Id);
========
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    rating = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tours", x => x.Id);
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    RentalCompanyId = table.Column<int>(type: "int", nullable: true)
========
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    RentalCompanyId = table.Column<int>(type: "int", nullable: false)
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_CarRentalCompanies_RentalCompanyId",
                        column: x => x.RentalCompanyId,
                        principalTable: "CarRentalCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                    AvailableSeats = table.Column<int>(type: "int", nullable: false),
                    DepartureAirport = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ArrivalAirport = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FlightId = table.Column<int>(type: "int", nullable: false)
========
                    AvailableStanderedSeats = table.Column<int>(type: "int", nullable: false),
                    AvailableBusinessSeats = table.Column<int>(type: "int", nullable: false),
                    AvailableFirstClassSeats = table.Column<int>(type: "int", nullable: false),
                    DepartureAirport = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ArrivalAirport = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FlightId = table.Column<int>(type: "int", nullable: true)
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flights_FlightCompanies_FlightId",
                        column: x => x.FlightId,
                        principalTable: "FlightCompanies",
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
========
                        principalColumn: "Id");
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    RoomType = table.Column<int>(type: "int", nullable: false),
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HotelId = table.Column<int>(type: "int", nullable: true)
========
                    HotelId = table.Column<int>(type: "int", nullable: false)
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_HotelCompanies_HotelId",
                        column: x => x.HotelId,
                        principalTable: "HotelCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                name: "Tours",
========
                name: "Trips",
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaxGuests = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: true),
========
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
                    TourCompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                    table.PrimaryKey("PK_Tours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tours_TourCompany_TourCompanyId",
                        column: x => x.TourCompanyId,
                        principalTable: "TourCompany",
========
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_Tours_TourCompanyId",
                        column: x => x.TourCompanyId,
                        principalTable: "Tours",
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    BookingType = table.Column<int>(type: "int", nullable: false),
                    RefId = table.Column<int>(type: "int", nullable: true),
                    PaymentId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                name: "RoomImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomImage_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TourId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourImages_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
========
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                    BookingId = table.Column<int>(type: "int", nullable: true)
========
                    BookingId = table.Column<int>(type: "int", nullable: false)
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_RentalCompanyId",
                table: "Cars",
                column: "RentalCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FlightId",
                table: "Flights",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingId",
                table: "Payments",
                column: "BookingId",
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                unique: true,
                filter: "[BookingId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoomImage_RoomId",
                table: "RoomImage",
                column: "RoomId");
========
                unique: true);
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HotelId",
                table: "Rooms",
                column: "HotelId");

            migrationBuilder.CreateIndex(
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                name: "IX_TourImages_TourId",
                table: "TourImages",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_TourCompanyId",
                table: "Tours",
========
                name: "IX_Trips_TourCompanyId",
                table: "Trips",
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
                column: "TourCompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                name: "RoomImage");

            migrationBuilder.DropTable(
                name: "TourImages");
========
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Trips");
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs

            migrationBuilder.DropTable(
                name: "CarRentalCompanies");

            migrationBuilder.DropTable(
                name: "FlightCompanies");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs
                name: "Rooms");
========
                name: "HotelCompanies");
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs

            migrationBuilder.DropTable(
                name: "Tours");

            migrationBuilder.DropTable(
                name: "Users");
<<<<<<<< HEAD:TravelBooking.Repository/Migrations/20250720125634_Initial Creation.cs

            migrationBuilder.DropTable(
                name: "HotelCompanies");

            migrationBuilder.DropTable(
                name: "TourCompany");
========
>>>>>>>> ayman:TravelBooking.Repository/Migrations/20250714134417_InitialCreate.cs
        }
    }
}
