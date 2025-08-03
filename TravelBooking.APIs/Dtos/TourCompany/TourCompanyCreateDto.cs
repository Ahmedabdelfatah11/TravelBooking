namespace TravelBooking.APIs.DTOS.TourCompany
{
    public class TourCompanyCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Location { get; set; }
        public string? Rating { get; set; }
        
        public string? AdminId { get; set; }
    }
}
