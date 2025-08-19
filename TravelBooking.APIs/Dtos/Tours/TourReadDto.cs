using TravelBooking.APIs.DTOS.TourTickets;

namespace TravelBooking.APIs.DTOS.Tours
{
    public class TourReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public string? Destination { get; set; }
        public int MaxGuests { get; set; }
        public int MinGroupSize { get; set; }
        public int MaxGroupSize { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? Category { get; set; }
        public string Languages { get; set; }
        public int? TourCompanyId { get; set; }
        public string? TourCompanyName { get; set; }
        public IEnumerable<string>? ImageUrls { get; set; } 


        public List<TourTicketDto>? Tickets { get; set; }
        public List<string> IncludedItems { get; set; } = new();
        public List<string> ExcludedItems { get; set; } = new();
        public List<TourQuestionDto>? Questions { get; set; } = new();
    }

}
