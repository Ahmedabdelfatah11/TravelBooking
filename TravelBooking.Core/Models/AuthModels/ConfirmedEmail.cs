using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Models
{
    public class ConfirmedEmail
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Code { get; set; }

    }
}
