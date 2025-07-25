using System.ComponentModel.DataAnnotations;

namespace TravelBooking.APIs.DTOS.Flight
{
    public class FlightDTO
    {
        public int FlightCompanyId { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public string AirlineName { get; set; } // Name of the airline operating the flight
        public string ImageUrl { get; set; } // URL to an image representing the flight or airline
        public int AvailableSeats { get; set; }
    }
}
