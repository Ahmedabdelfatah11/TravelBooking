
namespace TravelBooking.APIs.DTOS.FlightCompany
{
    public class FlightCompanyDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; } // Additional details about the flight company
        public int Rating { get; set; } // Rating out of 5 or a descriptive rating like "Excellent", "Good", etc.
        public int? FlightCount { get; set; }
    }
}
