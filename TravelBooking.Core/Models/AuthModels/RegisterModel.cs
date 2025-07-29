using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TravelBooking.APIs.Validation;

namespace TravelBooking.Models
{
    public class RegisterModel
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string UserName { get; set; } = null!;

        [Required, MaxLength(50), EmailAddress]
        public string Email { get; set; } = null!;
        [Phone, MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(200)]
        public string Address { get; set; } = null!;
        [AgeRange(16, 110)]

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } 


        [Required, MaxLength(50), DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}