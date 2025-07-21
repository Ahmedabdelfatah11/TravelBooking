using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Models.ResetPassword
{
    public class ForgotPasswordModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
