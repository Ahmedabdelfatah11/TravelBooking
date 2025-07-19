namespace TravelBooking.APIs.Dtos.TourCompany
{
    public class TourCompanyReadDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Location { get; set; }
        public string? Rating { get; set; }

        public List<TourSummaryDto>? Tours { get; set; }
    }
}
