namespace TravelBooking.APIs.DTOs
{
    public class FlightCompanyDetailsDTO
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Rating { get; set; } // Rating out of 5 or a descriptive rating like "Excellent", "Good", etc.
        public string FlightCount { get; set; } 
        public string Description { get; set; } // Additional details about the flight company
       public List<FlightDTO> Flights { get; set; } = new List<FlightDTO>(); // List of flights associated with the company
    }
}
