using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Models
{
    public class RegisterModel
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required, MaxLength(50)]
        public string UserName { get; set; }  

        [Required, MaxLength(50), EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(50), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}