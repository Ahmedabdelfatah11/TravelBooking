
using TravelBooking.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;


namespace TravelBooking.Repository.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations for entities
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarRentalCompany> CarRentalCompanies { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightCompany> FlightCompanies { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<HotelCompany> HotelCompanies { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourCompany> TourCompanies { get; set; }
        public DbSet<TourImage> TourImages { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<Favoritet> Favorites { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
    }
}