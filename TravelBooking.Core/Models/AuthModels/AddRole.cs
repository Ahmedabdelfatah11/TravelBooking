using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Models
{
    public class AddRole
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
