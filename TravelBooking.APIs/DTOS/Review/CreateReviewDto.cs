namespace TravelBooking.APIs.DTOS.Review
{
    public class CreateReviewDto
    {
        [Required(ErrorMessage = "Company type is required")]
        [StringLength(50, ErrorMessage = "Company type cannot exceed 50 characters")]
        public string CompanyType { get; set; } = string.Empty;

        public int? HotelCompanyId { get; set; }
        public int? FlightCompanyId { get; set; }
        public int? CarRentalCompanyId { get; set; }
        public int? TourCompanyId { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
        public string? Comment { get; set; }

        public bool IsValid()
        {
            var companyIds = new[] { HotelCompanyId, FlightCompanyId, CarRentalCompanyId, TourCompanyId };
            return companyIds.Count(id => id.HasValue) == 1;
        }

        public int GetCompanyId()
        {
            return HotelCompanyId ?? FlightCompanyId ?? CarRentalCompanyId ?? TourCompanyId ?? 0;
        }
    }
}
