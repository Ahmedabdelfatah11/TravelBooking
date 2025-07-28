using System.ComponentModel.DataAnnotations;

namespace TravelBooking.APIs.DTOS.Flight
{
    public class FlightDTO
    {
        public int FlightId { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string? AirlineName { get; set; } // Name of the airline operating the flight
        public string? ImageUrl { get; set; } // URL to an image representing the flight or airline
        public int EconomySeats { get; set; }
        public int BusinessSeats { get; set; }
        public int FirstClassSeats { get; set; }
        public int flightCompanyId { get; set; }
        public decimal EconomyPrice { get; set; }
        public decimal BusinessPrice { get; set; }
        public decimal FirstClassPrice { get; set; }
    }
}
