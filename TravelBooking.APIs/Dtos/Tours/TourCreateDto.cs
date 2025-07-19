using TravelBooking.Core.Models;

namespace TravelBooking.APIs.Dtos.Tours
{
    public class TourCreateDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public string? Destination { get; set; }
        public int MaxGuests { get; set; }
        public decimal Price { get; set; }
        public TourCategory? Category { get; set; }
        public int TourCompanyId { get; set; }
        public string? TourCompanyName { get; set; }
    }
}
