using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public class FlightCompany:BaseEntity
    {
        public string Name { get; set; } 
        public string Description { get; set; } // Description of the flight company
        public string ImageUrl { get; set; } // URL to the company's logo or image
        public String Location { get; set; } // Location of the flight company, e.g., "New York, USA"
        public string Rating { get; set; } // Rating out of 5 or a descriptive rating like "Excellent", "Good", etc.
        public ICollection<Flight> Flights { get; set; } // Navigation property for related flights
    } 
}
