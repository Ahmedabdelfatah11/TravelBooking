using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public class Flight: BaseEntity
    {
        public DateTime DepartureTime { get; set; } // Departure date and time
        public DateTime ArrivalTime { get; set; } // Arrival date and time
        public decimal Price { get; set; } // Price of the flight ticket
        public int AvailableStanderedSeats { get; set; } // Number of available seats on the flight
        public int AvailableBusinessSeats { get; set; } // Number of available business class seats on the flight
        public int AvailableFirstClassSeats { get; set; } // Number of available first class seats on the flight
        public string DepartureAirport { get; set; } // Departure airport code or name
        public string ArrivalAirport { get; set; } // Arrival airport code or name
    } 
}
