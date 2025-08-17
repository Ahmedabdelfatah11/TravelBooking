using TravelBooking.APIs.Validation;

namespace TravelBooking.APIs.DTOS.UserProfile
{
    public class UpdateUserProfileDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        [AgeRange(16, 110)]
        public DateTime? DateOfBirth { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
