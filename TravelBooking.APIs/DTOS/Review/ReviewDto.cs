namespace TravelBooking.APIs.DTOS.Review
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string CompanyType { get; set; } = string.Empty;
        public int? HotelCompanyId { get; set; }
        public int? FlightCompanyId { get; set; }
        public int? CarRentalCompanyId { get; set; }
        public int? TourCompanyId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Company Information
        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyImageUrl { get; set; }
        public string? CompanyLocation { get; set; }

        // User Information
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
    }
}
