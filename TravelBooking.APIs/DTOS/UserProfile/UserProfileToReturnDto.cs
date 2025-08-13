using System.IdentityModel.Tokens.Jwt;
using TravelBooking.APIs.Validation;

namespace TravelBooking.APIs.DTOS.UserProfile
{
    public class UserProfileToReturnDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string Token { get; set; } = null!;
    }
}
