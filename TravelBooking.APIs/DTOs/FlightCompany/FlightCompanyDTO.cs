
namespace TravelBooking.APIs.DTOS.FlightCompany
{
    public class FlightCompanyDTO
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Rating { get; set; } // Rating out of 5 or a descriptive rating like "Excellent", "Good", etc.
        public string FlightCount { get; set; }
    }
}
