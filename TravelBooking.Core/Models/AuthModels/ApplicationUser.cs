using Microsoft.AspNetCore.Identity;
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

    }
}
