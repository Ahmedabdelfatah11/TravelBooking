
//using System;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
//using TravelBooking.Repository.Data;

//#nullable disable

//namespace TravelBooking.Repository.Migrations
//{
//    [DbContext(typeof(AppDbContext))]
//    partial class AppDbContextModelSnapshot : ModelSnapshot
//    {
//        protected override void BuildModel(ModelBuilder modelBuilder)
//        {
//#pragma warning disable 612, 618
//            modelBuilder
//                .HasAnnotation("ProductVersion", "9.0.7")
//                .HasAnnotation("Relational:MaxIdentifierLength", 128);

//            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
//                {
//                    b.Property<string>("Id")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("ConcurrencyStamp")
//                        .IsConcurrencyToken()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("Name")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.Property<string>("NormalizedName")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.HasKey("Id");

//                    b.HasIndex("NormalizedName")
//                        .IsUnique()
//                        .HasDatabaseName("RoleNameIndex")
//                        .HasFilter("[NormalizedName] IS NOT NULL");

//                    b.ToTable("AspNetRoles", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("ClaimType")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("ClaimValue")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("RoleId")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(450)");

//                    b.HasKey("Id");

//                    b.HasIndex("RoleId");

//                    b.ToTable("AspNetRoleClaims", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("ClaimType")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("ClaimValue")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("UserId")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(450)");

//                    b.HasKey("Id");

//                    b.HasIndex("UserId");

//                    b.ToTable("AspNetUserClaims", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
//                {
//                    b.Property<string>("LoginProvider")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("ProviderKey")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("ProviderDisplayName")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("UserId")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(450)");

//                    b.HasKey("LoginProvider", "ProviderKey");

//                    b.HasIndex("UserId");

//                    b.ToTable("AspNetUserLogins", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
//                {
//                    b.Property<string>("UserId")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("RoleId")
//                        .HasColumnType("nvarchar(450)");

//                    b.HasKey("UserId", "RoleId");

//                    b.HasIndex("RoleId");

//                    b.ToTable("AspNetUserRoles", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
//                {
//                    b.Property<string>("UserId")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("LoginProvider")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("Name")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("Value")
//                        .HasColumnType("nvarchar(max)");

//                    b.HasKey("UserId", "LoginProvider", "Name");

//                    b.ToTable("AspNetUserTokens", (string)null);
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Booking", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<int>("BookingType")
//                        .HasColumnType("int");

//                    b.Property<int?>("CarId")
//                        .HasColumnType("int");

//                    b.Property<DateTime>("EndDate")
//                        .HasColumnType("datetime2");

//                    b.Property<int?>("FlightId")
//                        .HasColumnType("int");

//                    b.Property<int?>("RoomId")
//                        .HasColumnType("int");

//                    b.Property<DateTime>("StartDate")
//                        .HasColumnType("datetime2");

//                    b.Property<int>("Status")
//                        .HasColumnType("int");

//                    b.Property<int?>("TourId")
//                        .HasColumnType("int");

//                    b.Property<string>("UserId")
//                        .HasColumnType("nvarchar(450)");

//                    b.HasKey("Id");

//                    b.HasIndex("CarId");

//                    b.HasIndex("FlightId");

//                    b.HasIndex("RoomId");

//                    b.HasIndex("TourId");

//                    b.HasIndex("UserId");

//                    b.ToTable("Bookings");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Car", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<DateTime>("ArrivalTime")
//                        .HasColumnType("datetime2");

//                    b.Property<int>("Capacity")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int")
//                        .HasDefaultValue(5);

//                    b.Property<DateTime>("DepartureTime")
//                        .HasColumnType("datetime2");

//                    b.Property<string>("Description")
//                        .HasMaxLength(1000)
//                        .HasColumnType("nvarchar(1000)");

//                    b.Property<string>("ImageUrl")
//                        .HasMaxLength(500)
//                        .HasColumnType("nvarchar(500)");

//                    b.Property<bool>("IsAvailable")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("bit")
//                        .HasDefaultValue(true);

//                    b.Property<string>("Location")
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Model")
//                        .IsRequired()
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<decimal?>("Price")
//                        .HasColumnType("decimal(18,2)");

//                    b.Property<int?>("RentalCompanyId")
//                        .HasColumnType("int");

//                    b.HasKey("Id");

//                    b.HasIndex("RentalCompanyId");

//                    b.ToTable("Cars");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.CarRentalCompany", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("ImageUrl")
//                        .HasMaxLength(500)
//                        .HasColumnType("nvarchar(500)");

//                    b.Property<string>("Location")
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Name")
//                        .IsRequired()
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Rating")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("description")
//                        .HasMaxLength(1000)
//                        .HasColumnType("nvarchar(1000)");

//                    b.HasKey("Id");

//                    b.ToTable("CarRentalCompanies");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Flight", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("ArrivalAirport")
//                        .IsRequired()
//                        .HasMaxLength(10)
//                        .HasColumnType("nvarchar(10)");

//                    b.Property<DateTime>("ArrivalTime")
//                        .HasColumnType("datetime2");

//                    b.Property<int>("AvailableSeats")
//                        .HasColumnType("int");

//                    b.Property<string>("DepartureAirport")
//                        .IsRequired()
//                        .HasMaxLength(10)
//                        .HasColumnType("nvarchar(10)");

//                    b.Property<DateTime>("DepartureTime")
//                        .HasColumnType("datetime2");

//                    b.Property<int>("FlightId")
//                        .HasColumnType("int");

//                    b.Property<decimal>("Price")
//                        .HasColumnType("decimal(18,2)");

//                    b.HasKey("Id");

//                    b.HasIndex("FlightId");

//                    b.ToTable("Flights");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.FlightCompany", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("Description")
//                        .HasMaxLength(1000)
//                        .HasColumnType("nvarchar(1000)");

//                    b.Property<string>("ImageUrl")
//                        .HasMaxLength(500)
//                        .HasColumnType("nvarchar(500)");

//                    b.Property<string>("Location")
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Name")
//                        .IsRequired()
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Rating")
//                        .HasColumnType("nvarchar(max)");

//                    b.HasKey("Id");

//                    b.ToTable("FlightCompanies");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.HotelCompany", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("Description")
//                        .IsRequired()
//                        .HasMaxLength(1000)
//                        .HasColumnType("nvarchar(1000)");

//                    b.Property<string>("ImageUrl")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("Location")
//                        .IsRequired()
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Name")
//                        .IsRequired()
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Rating")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.HasKey("Id");

//                    b.ToTable("HotelCompanies");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Payment", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<decimal>("Amount")
//                        .HasColumnType("decimal(18,2)");

//                    b.Property<int?>("BookingId")
//                        .HasColumnType("int");

//                    b.Property<DateTime>("PaymentDate")
//                        .HasColumnType("datetime2");

//                    b.Property<string>("PaymentMethod")
//                        .IsRequired()
//                        .HasMaxLength(50)
//                        .HasColumnType("nvarchar(50)");

//                    b.Property<int>("PaymentStatus")
//                        .HasColumnType("int");

//                    b.Property<string>("TransactionId")
//                        .IsRequired()
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.HasKey("Id");

//                    b.HasIndex("BookingId")
//                        .IsUnique()
//                        .HasFilter("[BookingId] IS NOT NULL");

//                    b.ToTable("Payments");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Room", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("Description")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<DateTime>("From")
//                        .HasColumnType("datetime2");

//                    b.Property<int?>("HotelId")
//                        .HasColumnType("int");

//                    b.Property<bool>("IsAvailable")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("bit")
//                        .HasDefaultValue(true);

//                    b.Property<decimal>("Price")
//                        .HasColumnType("decimal(18,2)");

//                    b.Property<int>("RoomType")
//                        .HasColumnType("int")
//                        .HasAnnotation("Relational:JsonPropertyName", "RoomType");

//                    b.Property<DateTime>("To")
//                        .HasColumnType("datetime2");

//                    b.HasKey("Id");

//                    b.HasIndex("HotelId");

//                    b.ToTable("Rooms");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.RoomImage", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("ImageUrl")
//                        .IsRequired()
//                        .HasMaxLength(500)
//                        .HasColumnType("nvarchar(500)");

//                    b.Property<int>("RoomId")
//                        .HasColumnType("int");

//                    b.HasKey("Id");

//                    b.HasIndex("RoomId");

//                    b.ToTable("RoomImage");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Tour", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<int?>("Category")
//                        .HasColumnType("int");

//                    b.Property<string>("Description")
//                        .HasMaxLength(1000)
//                        .HasColumnType("nvarchar(1000)");

//                    b.Property<string>("Destination")
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<DateTime>("EndDate")
//                        .HasColumnType("datetime2");

//                    b.Property<int>("MaxGuests")
//                        .HasColumnType("int");

//                    b.Property<string>("Name")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<decimal>("Price")
//                        .HasColumnType("decimal(18,2)");

//                    b.Property<DateTime>("StartDate")
//                        .HasColumnType("datetime2");

//                    b.Property<int?>("TourCompanyId")
//                        .HasColumnType("int");

//                    b.HasKey("Id");

//                    b.HasIndex("TourCompanyId");

//                    b.ToTable("Tours");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.TourCompany", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("Description")
//                        .HasMaxLength(1000)
//                        .HasColumnType("nvarchar(1000)");

//                    b.Property<string>("ImageUrl")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("Location")
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Name")
//                        .IsRequired()
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("rating")
//                        .HasColumnType("nvarchar(max)");

//                    b.HasKey("Id");

//                    b.ToTable("TourCompanies");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.TourImage", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("ImageUrl")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<int>("TourId")
//                        .HasColumnType("int");

//                    b.HasKey("Id");

//                    b.HasIndex("TourId");

//                    b.ToTable("TourImages");
//                });

//            modelBuilder.Entity("TravelBooking.Models.ApplicationUser", b =>
//                {
//                    b.Property<string>("Id")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<int>("AccessFailedCount")
//                        .HasColumnType("int");

//                    b.Property<string>("ConcurrencyStamp")
//                        .IsConcurrencyToken()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("Email")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.Property<bool>("EmailConfirmed")
//                        .HasColumnType("bit");

//                    b.Property<string>("FirstName")
//                        .IsRequired()
//                        .HasMaxLength(50)
//                        .HasColumnType("nvarchar(50)");

//                    b.Property<string>("LastName")
//                        .IsRequired()
//                        .HasMaxLength(50)
//                        .HasColumnType("nvarchar(50)");

//                    b.Property<bool>("LockoutEnabled")
//                        .HasColumnType("bit");

//                    b.Property<DateTimeOffset?>("LockoutEnd")
//                        .HasColumnType("datetimeoffset");

//                    b.Property<string>("NormalizedEmail")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.Property<string>("NormalizedUserName")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.Property<string>("PasswordHash")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("PhoneNumber")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<bool>("PhoneNumberConfirmed")
//                        .HasColumnType("bit");

//                    b.Property<string>("SecurityStamp")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<bool>("TwoFactorEnabled")
//                        .HasColumnType("bit");

//                    b.Property<string>("UserName")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.HasKey("Id");

//                    b.HasIndex("NormalizedEmail")
//                        .HasDatabaseName("EmailIndex");

//                    b.HasIndex("NormalizedUserName")
//                        .IsUnique()
//                        .HasDatabaseName("UserNameIndex")
//                        .HasFilter("[NormalizedUserName] IS NOT NULL");

//                    b.ToTable("AspNetUsers", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
//                {
//                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
//                        .WithMany()
//                        .HasForeignKey("RoleId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
//                {
//                    b.HasOne("TravelBooking.Models.ApplicationUser", null)
//                        .WithMany()
//                        .HasForeignKey("UserId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
//                {
//                    b.HasOne("TravelBooking.Models.ApplicationUser", null)
//                        .WithMany()
//                        .HasForeignKey("UserId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
//                {
//                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
//                        .WithMany()
//                        .HasForeignKey("RoleId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();

//                    b.HasOne("TravelBooking.Models.ApplicationUser", null)
//                        .WithMany()
//                        .HasForeignKey("UserId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
//                {
//                    b.HasOne("TravelBooking.Models.ApplicationUser", null)
//                        .WithMany()
//                        .HasForeignKey("UserId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Booking", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.Car", "Car")
//                        .WithMany()
//                        .HasForeignKey("CarId")
//                        .OnDelete(DeleteBehavior.Restrict);

//                    b.HasOne("TravelBooking.Core.Models.Flight", "Flight")
//                        .WithMany()
//                        .HasForeignKey("FlightId")
//                        .OnDelete(DeleteBehavior.Restrict);

//                    b.HasOne("TravelBooking.Core.Models.Room", "Room")
//                        .WithMany()
//                        .HasForeignKey("RoomId")
//                        .OnDelete(DeleteBehavior.Restrict);

//                    b.HasOne("TravelBooking.Core.Models.Tour", "Tour")
//                        .WithMany()
//                        .HasForeignKey("TourId")
//                        .OnDelete(DeleteBehavior.Restrict);

//                    b.HasOne("TravelBooking.Models.ApplicationUser", "User")
//                        .WithMany("bookings")
//                        .HasForeignKey("UserId");

//                    b.Navigation("Car");

//                    b.Navigation("Flight");

//                    b.Navigation("Room");

//                    b.Navigation("Tour");

//                    b.Navigation("User");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Car", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.CarRentalCompany", "RentalCompany")
//                        .WithMany("Cars")
//                        .HasForeignKey("RentalCompanyId")
//                        .OnDelete(DeleteBehavior.Cascade);

//                    b.Navigation("RentalCompany");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Flight", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.FlightCompany", "FlightCompany")
//                        .WithMany("Flights")
//                        .HasForeignKey("FlightId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();

//                    b.Navigation("FlightCompany");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Payment", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.Booking", "Booking")
//                        .WithOne("Payment")
//                        .HasForeignKey("TravelBooking.Core.Models.Payment", "BookingId")
//                        .OnDelete(DeleteBehavior.Cascade);

//                    b.Navigation("Booking");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Room", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.HotelCompany", "Hotel")
//                        .WithMany("Rooms")
//                        .HasForeignKey("HotelId")
//                        .OnDelete(DeleteBehavior.Cascade);

//                    b.Navigation("Hotel");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.RoomImage", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.Room", "Room")
//                        .WithMany("Images")
//                        .HasForeignKey("RoomId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();

//                    b.Navigation("Room");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Tour", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.TourCompany", "TourCompany")
//                        .WithMany("Tours")
//                        .HasForeignKey("TourCompanyId")
//                        .OnDelete(DeleteBehavior.Restrict);

//                    b.Navigation("TourCompany");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.TourImage", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.Tour", "Tour")
//                        .WithMany("TourImages")
//                        .HasForeignKey("TourId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();

//                    b.Navigation("Tour");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Booking", b =>
//                {
//                    b.Navigation("Payment");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.CarRentalCompany", b =>
//                {
//                    b.Navigation("Cars");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.FlightCompany", b =>
//                {
//                    b.Navigation("Flights");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.HotelCompany", b =>
//                {
//                    b.Navigation("Rooms");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Room", b =>
//                {
//                    b.Navigation("Images");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Tour", b =>
//                {
//                    b.Navigation("TourImages");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.TourCompany", b =>
//                {
//                    b.Navigation("Tours");
//                });

//            modelBuilder.Entity("TravelBooking.Models.ApplicationUser", b =>
//                {
//                    b.Navigation("bookings");
//                });
//#pragma warning restore 612, 618
//        }
//    }
//}
//=======
//﻿// <auto-generated />
//using System;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
//using TravelBooking.Repository.Data;

//#nullable disable

//namespace TravelBooking.Repository.Migrations
//{
//    [DbContext(typeof(AppDbContext))]
//    partial class AppDbContextModelSnapshot : ModelSnapshot
//    {
//        protected override void BuildModel(ModelBuilder modelBuilder)
//        {
//#pragma warning disable 612, 618
//            modelBuilder
//                .HasAnnotation("ProductVersion", "9.0.7")
//                .HasAnnotation("Relational:MaxIdentifierLength", 128);

//            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
//                {
//                    b.Property<string>("Id")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("ConcurrencyStamp")
//                        .IsConcurrencyToken()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("Name")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.Property<string>("NormalizedName")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.HasKey("Id");

//                    b.HasIndex("NormalizedName")
//                        .IsUnique()
//                        .HasDatabaseName("RoleNameIndex")
//                        .HasFilter("[NormalizedName] IS NOT NULL");

//                    b.ToTable("AspNetRoles", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("ClaimType")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("ClaimValue")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("RoleId")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(450)");

//                    b.HasKey("Id");

//                    b.HasIndex("RoleId");

//                    b.ToTable("AspNetRoleClaims", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("ClaimType")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("ClaimValue")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("UserId")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(450)");

//                    b.HasKey("Id");

//                    b.HasIndex("UserId");

//                    b.ToTable("AspNetUserClaims", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
//                {
//                    b.Property<string>("LoginProvider")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("ProviderKey")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("ProviderDisplayName")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("UserId")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(450)");

//                    b.HasKey("LoginProvider", "ProviderKey");

//                    b.HasIndex("UserId");

//                    b.ToTable("AspNetUserLogins", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
//                {
//                    b.Property<string>("UserId")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("RoleId")
//                        .HasColumnType("nvarchar(450)");

//                    b.HasKey("UserId", "RoleId");

//                    b.HasIndex("RoleId");

//                    b.ToTable("AspNetUserRoles", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
//                {
//                    b.Property<string>("UserId")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("LoginProvider")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("Name")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<string>("Value")
//                        .HasColumnType("nvarchar(max)");

//                    b.HasKey("UserId", "LoginProvider", "Name");

//                    b.ToTable("AspNetUserTokens", (string)null);
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Booking", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<int>("BookingType")
//                        .HasColumnType("int");

//                    b.Property<DateTime>("EndDate")
//                        .HasColumnType("datetime2");

//                    b.Property<int?>("PaymentId")
//                        .HasColumnType("int");

//                    b.Property<int?>("RefId")
//                        .HasColumnType("int");

//                    b.Property<DateTime>("StartDate")
//                        .HasColumnType("datetime2");

//                    b.Property<int>("UserId")
//                        .HasColumnType("int");

//                    b.Property<int>("status")
//                        .HasColumnType("int");

//                    b.HasKey("Id");

//                    b.HasIndex("UserId");

//                    b.ToTable("Bookings", (string)null);
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Car", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<int>("Capacity")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int")
//                        .HasDefaultValue(5);

//                    b.Property<string>("Description")
//                        .HasMaxLength(1000)
//                        .HasColumnType("nvarchar(1000)");

//                    b.Property<string>("ImageUrl")
//                        .HasMaxLength(500)
//                        .HasColumnType("nvarchar(500)");

//                    b.Property<bool>("IsAvailable")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("bit")
//                        .HasDefaultValue(true);

//                    b.Property<string>("Location")
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Model")
//                        .IsRequired()
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<decimal?>("Price")
//                        .HasColumnType("decimal(18,2)");

//                    b.Property<int?>("RentalCompanyId")
//                        .HasColumnType("int");

//                    b.HasKey("Id");

//                    b.HasIndex("RentalCompanyId");

//                    b.ToTable("Cars", (string)null);
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.CarRentalCompany", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("ImageUrl")
//                        .HasMaxLength(500)
//                        .HasColumnType("nvarchar(500)");

//                    b.Property<string>("Location")
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Name")
//                        .IsRequired()
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Rating")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("description")
//                        .HasMaxLength(1000)
//                        .HasColumnType("nvarchar(1000)");

//                    b.HasKey("Id");

//                    b.ToTable("CarRentalCompanies", (string)null);
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Flight", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("ArrivalAirport")
//                        .IsRequired()
//                        .HasMaxLength(10)
//                        .HasColumnType("nvarchar(10)");

//                    b.Property<DateTime>("ArrivalTime")
//                        .HasColumnType("datetime2");

//                    b.Property<int>("AvailableSeats")
//                        .HasColumnType("int");

//                    b.Property<string>("DepartureAirport")
//                        .IsRequired()
//                        .HasMaxLength(10)
//                        .HasColumnType("nvarchar(10)");

//                    b.Property<DateTime>("DepartureTime")
//                        .HasColumnType("datetime2");

//                    b.Property<int>("FlightId")
//                        .HasColumnType("int");

//                    b.Property<decimal>("Price")
//                        .HasColumnType("decimal(18,2)");

//                    b.HasKey("Id");

//                    b.HasIndex("FlightId");

//                    b.ToTable("Flights", (string)null);
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.FlightCompany", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("Description")
//                        .HasMaxLength(1000)
//                        .HasColumnType("nvarchar(1000)");

//                    b.Property<string>("ImageUrl")
//                        .HasMaxLength(500)
//                        .HasColumnType("nvarchar(500)");

//                    b.Property<string>("Location")
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Name")
//                        .IsRequired()
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Rating")
//                        .HasColumnType("nvarchar(max)");

//                    b.HasKey("Id");

//                    b.ToTable("FlightCompanies", (string)null);
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.HotelCompany", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("Description")
//                        .IsRequired()
//                        .HasMaxLength(1000)
//                        .HasColumnType("nvarchar(1000)");

//                    b.Property<string>("ImageUrl")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("Location")
//                        .IsRequired()
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Name")
//                        .IsRequired()
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Rating")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.HasKey("Id");

//                    b.ToTable("HotelCompanies", (string)null);
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Payment", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<decimal>("Amount")
//                        .HasColumnType("decimal(18,2)");

//                    b.Property<int?>("BookingId")
//                        .HasColumnType("int");

//                    b.Property<DateTime>("PaymentDate")
//                        .HasColumnType("datetime2");

//                    b.Property<string>("PaymentMethod")
//                        .IsRequired()
//                        .HasMaxLength(50)
//                        .HasColumnType("nvarchar(50)");

//                    b.Property<int>("PaymentStatus")
//                        .HasColumnType("int");

//                    b.Property<string>("TransactionId")
//                        .IsRequired()
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.HasKey("Id");

//                    b.HasIndex("BookingId")
//                        .IsUnique()
//                        .HasFilter("[BookingId] IS NOT NULL");

//                    b.ToTable("Payments", (string)null);
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Room", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("Description")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<int?>("HotelId")
//                        .HasColumnType("int");

//                    b.Property<bool>("IsAvailable")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("bit")
//                        .HasDefaultValue(true);

//                    b.Property<decimal>("Price")
//                        .HasColumnType("decimal(18,2)");

//                    b.Property<int>("RoomType")
//                        .HasColumnType("int")
//                        .HasAnnotation("Relational:JsonPropertyName", "RoomType");

//                    b.HasKey("Id");

//                    b.HasIndex("HotelId");

//                    b.ToTable("Rooms", (string)null);
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.RoomImage", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("ImageUrl")
//                        .IsRequired()
//                        .HasMaxLength(500)
//                        .HasColumnType("nvarchar(500)");

//                    b.Property<int>("RoomId")
//                        .HasColumnType("int");

//                    b.HasKey("Id");

//                    b.HasIndex("RoomId");

//                    b.ToTable("RoomImage", (string)null);
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Tour", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<int?>("Category")
//                        .HasColumnType("int");

//                    b.Property<string>("Description")
//                        .HasMaxLength(1000)
//                        .HasColumnType("nvarchar(1000)");

//                    b.Property<string>("Destination")
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<DateTime>("EndDate")
//                        .HasColumnType("datetime2");

//                    b.Property<int>("MaxGuests")
//                        .HasColumnType("int");

//                    b.Property<string>("Name")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<decimal>("Price")
//                        .HasColumnType("decimal(18,2)");

//                    b.Property<DateTime>("StartDate")
//                        .HasColumnType("datetime2");

//                    b.Property<int?>("TourCompanyId")
//                        .HasColumnType("int");

//                    b.HasKey("Id");

//                    b.HasIndex("TourCompanyId");

//                    b.ToTable("Tours", (string)null);
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.TourCompany", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("Description")
//                        .HasMaxLength(1000)
//                        .HasColumnType("nvarchar(1000)");

//                    b.Property<string>("ImageUrl")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("Location")
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("Name")
//                        .IsRequired()
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("rating")
//                        .HasColumnType("nvarchar(max)");

//                    b.HasKey("Id");

//                    b.ToTable("TourCompany", (string)null);
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.TourImage", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("ImageUrl")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<int>("TourId")
//                        .HasColumnType("int");

//                    b.HasKey("Id");

//                    b.HasIndex("TourId");

//                    b.ToTable("TourImages", (string)null);
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.User", b =>
//                {
//                    b.Property<int>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasColumnType("int");

//                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

//                    b.Property<string>("Email")
//                        .IsRequired()
//                        .HasMaxLength(100)
//                        .HasColumnType("nvarchar(100)");

//                    b.Property<string>("FirstName")
//                        .IsRequired()
//                        .HasMaxLength(50)
//                        .HasColumnType("nvarchar(50)");

//                    b.Property<string>("LastName")
//                        .IsRequired()
//                        .HasMaxLength(50)
//                        .HasColumnType("nvarchar(50)");

//                    b.Property<string>("Password")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("Phone")
//                        .IsRequired()
//                        .HasMaxLength(20)
//                        .HasColumnType("nvarchar(20)");

//                    b.HasKey("Id");

//                    b.ToTable("Users", (string)null);
//                });

//            modelBuilder.Entity("TravelBooking.Models.ApplicationUser", b =>
//                {
//                    b.Property<string>("Id")
//                        .HasColumnType("nvarchar(450)");

//                    b.Property<int>("AccessFailedCount")
//                        .HasColumnType("int");

//                    b.Property<string>("ConcurrencyStamp")
//                        .IsConcurrencyToken()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("Email")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.Property<bool>("EmailConfirmed")
//                        .HasColumnType("bit");

//                    b.Property<string>("FirstName")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("LastName")
//                        .IsRequired()
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<bool>("LockoutEnabled")
//                        .HasColumnType("bit");

//                    b.Property<DateTimeOffset?>("LockoutEnd")
//                        .HasColumnType("datetimeoffset");

//                    b.Property<string>("NormalizedEmail")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.Property<string>("NormalizedUserName")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.Property<string>("PasswordHash")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<string>("PhoneNumber")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<bool>("PhoneNumberConfirmed")
//                        .HasColumnType("bit");

//                    b.Property<string>("SecurityStamp")
//                        .HasColumnType("nvarchar(max)");

//                    b.Property<bool>("TwoFactorEnabled")
//                        .HasColumnType("bit");

//                    b.Property<string>("UserName")
//                        .HasMaxLength(256)
//                        .HasColumnType("nvarchar(256)");

//                    b.HasKey("Id");

//                    b.HasIndex("NormalizedEmail")
//                        .HasDatabaseName("EmailIndex");

//                    b.HasIndex("NormalizedUserName")
//                        .IsUnique()
//                        .HasDatabaseName("UserNameIndex")
//                        .HasFilter("[NormalizedUserName] IS NOT NULL");

//                    b.ToTable("AspNetUsers", (string)null);
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
//                {
//                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
//                        .WithMany()
//                        .HasForeignKey("RoleId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
//                {
//                    b.HasOne("TravelBooking.Models.ApplicationUser", null)
//                        .WithMany()
//                        .HasForeignKey("UserId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
//                {
//                    b.HasOne("TravelBooking.Models.ApplicationUser", null)
//                        .WithMany()
//                        .HasForeignKey("UserId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
//                {
//                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
//                        .WithMany()
//                        .HasForeignKey("RoleId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();

//                    b.HasOne("TravelBooking.Models.ApplicationUser", null)
//                        .WithMany()
//                        .HasForeignKey("UserId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });

//            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
//                {
//                    b.HasOne("TravelBooking.Models.ApplicationUser", null)
//                        .WithMany()
//                        .HasForeignKey("UserId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Booking", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.User", "User")
//                        .WithMany("bookings")
//                        .HasForeignKey("UserId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();

//                    b.Navigation("User");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Car", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.CarRentalCompany", "RentalCompany")
//                        .WithMany("Cars")
//                        .HasForeignKey("RentalCompanyId")
//                        .OnDelete(DeleteBehavior.Cascade);

//                    b.Navigation("RentalCompany");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Flight", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.FlightCompany", "FlightCompany")
//                        .WithMany("Flights")
//                        .HasForeignKey("FlightId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();

//                    b.Navigation("FlightCompany");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Payment", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.Booking", "Booking")
//                        .WithOne("Payment")
//                        .HasForeignKey("TravelBooking.Core.Models.Payment", "BookingId")
//                        .OnDelete(DeleteBehavior.Cascade);

//                    b.Navigation("Booking");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Room", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.HotelCompany", "Hotel")
//                        .WithMany("Rooms")
//                        .HasForeignKey("HotelId")
//                        .OnDelete(DeleteBehavior.Cascade);

//                    b.Navigation("Hotel");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.RoomImage", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.Room", "Room")
//                        .WithMany("Images")
//                        .HasForeignKey("RoomId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();

//                    b.Navigation("Room");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Tour", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.TourCompany", "TourCompany")
//                        .WithMany("Tours")
//                        .HasForeignKey("TourCompanyId")
//                        .OnDelete(DeleteBehavior.Restrict);

//                    b.Navigation("TourCompany");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.TourImage", b =>
//                {
//                    b.HasOne("TravelBooking.Core.Models.Tour", "Tour")
//                        .WithMany("TourImages")
//                        .HasForeignKey("TourId")
//                        .OnDelete(DeleteBehavior.Cascade)
//                        .IsRequired();

//                    b.Navigation("Tour");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Booking", b =>
//                {
//                    b.Navigation("Payment");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.CarRentalCompany", b =>
//                {
//                    b.Navigation("Cars");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.FlightCompany", b =>
//                {
//                    b.Navigation("Flights");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.HotelCompany", b =>
//                {
//                    b.Navigation("Rooms");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Room", b =>
//                {
//                    b.Navigation("Images");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.Tour", b =>
//                {
//                    b.Navigation("TourImages");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.TourCompany", b =>
//                {
//                    b.Navigation("Tours");
//                });

//            modelBuilder.Entity("TravelBooking.Core.Models.User", b =>
//                {
//                    b.Navigation("bookings");
//                });
//#pragma warning restore 612, 618
//        }
//    }
//}
//>>>>>>> cb173da (- implemented role seeding for Admin and User roles)
