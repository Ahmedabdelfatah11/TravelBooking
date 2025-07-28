namespace TravelBooking.APIs.DTOS.Tours
{
    public class TourReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public string? Destination { get; set; }
        public int MaxGuests { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }
        public int? TourCompanyId { get; set; }
        public string? TourCompanyName { get; set; }

        public List<string>? ImageUrls { get; set; }
    }
}
