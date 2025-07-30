using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.ComponentModel.DataAnnotations;
using TravelBooking.Core.Models;

namespace TravelBooking.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public ICollection<Booking> bookings { get; set; } = new List<Booking>();

        public ICollection<Favoritet> favoritets  { get; set; } = new List<Favoritet>();

        public ICollection<Review> reviews { get; set; } = new List<Review>();

        public HotelCompany? HotelCompany { get; set; }
        public FlightCompany? FlightCompany { get; set; }
        public TourCompany? TourCompany { get; set; }
        public CarRentalCompany? CarRentalCompany { get; set; }

    }
}
