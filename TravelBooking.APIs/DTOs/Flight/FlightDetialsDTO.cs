using System.ComponentModel.DataAnnotations;
using TravelBooking.APIs.DTOS.FlightCompany;

namespace TravelBooking.APIs.DTOS.Flight
{
    public class FlightDetialsDTO
    {
        [Required]

        public int FlightNumber { get; set; } // Flight number, e.g., "AA123"
        public string DepartureAirport { get; set; } // Departure airport code, e.g., "JFK"
        public string ArrivalAirport { get; set; } // Arrival airport code, e.g., "LAX"
        public DateTime departureTime { get; set; } // Departure time of the flight
        public DateTime arrivalTime { get; set; } // Arrival time of the flight
        
        public int EconomySeats { get; set; }
        public int BusinessSeats { get; set; }
        public int FirstClassSeats { get; set; }

        public decimal EconomyPrice { get; set; }
        public decimal BusinessPrice { get; set; }
        public decimal FirstClassPrice { get; set; } 
        public FlightCompanyDTO FlightCompany { get; set; }

    }
}
