namespace TravelBooking.APIs.Dtos.TourCompany
{
    public class TourSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Destination { get; set; }
        public int MaxGuests { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string>? ImageUrls { get; set; }

    }
}
