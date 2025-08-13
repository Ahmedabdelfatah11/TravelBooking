namespace TravelBooking.APIs.DTOS.TourCompany
{
    public class TourCompanyReadDto
    {
        public string Name { get; set; } 
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Location { get; set; }
        public int? Rating { get; set; }

        public List<TourSummaryDto>? Tours { get; set; }
    }
}
