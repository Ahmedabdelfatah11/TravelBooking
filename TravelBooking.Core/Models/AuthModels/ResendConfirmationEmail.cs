using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Models
{
    public class ResendConfirmationEmail
    {
        [Required(ErrorMessage = "Email is required."),DataType(DataType.EmailAddress)]
        public string Email { get; set; } 
    }
}
