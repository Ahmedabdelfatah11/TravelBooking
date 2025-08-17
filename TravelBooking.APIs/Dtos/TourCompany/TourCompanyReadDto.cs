namespace TravelBooking.APIs.DTOS.TourCompany
{
    public class TourCompanyReadDto
    {
        public int Id { get; set; }      
        public string Name { get; set; } 
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Location { get; set; }
        public int? Rating { get; set; }
        public string? AdminId { get; set; } // ✅ Include this
        public List<TourSummaryDto>? Tours { get; set; }
    }
}
