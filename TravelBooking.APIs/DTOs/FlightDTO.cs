﻿using System.ComponentModel.DataAnnotations;

namespace TravelBooking.APIs.DTOs
{
    public class FlightDTO
    {
        [Required]
       
        public int FlightNumber { get; set; } // Flight number, e.g., "AA123"
        public string DepartureAirport { get; set; } // Departure airport code, e.g., "JFK"
        public string ArrivalAirport { get; set; } // Arrival airport code, e.g., "LAX"
        public DateTime departureTime { get; set; } // Departure time of the flight
        public DateTime arrivalTime { get; set; } // Arrival time of the flight
        public decimal Price { get; set; } // Price of the flight ticket
        public string AirlineName { get; set; } // Name of the airline operating the flight
        public string ImageUrl { get; set; } // URL to an image representing the flight or airline

        public string rating { get; set; } // descriptive rating like "Excellent", "Good", etc.
        //public int count { get; set; } // Num of Flights
    }
}
