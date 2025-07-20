using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.DTOS.Cars;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.DTOS.CarRentalCompanies
{
    public class CarRentalDto
    {
        public string? Name { get; set; } // Name of the car rental company
        public string? Description { get; set; } // Description of the car rental company
        public string? Location { get; set; } // Location of the car rental company, e.g., "Los Angeles, USA"
        public string? ImageUrl { get; set; } // URL to the company's logo or image
        public string? Rating { get; set; } // Rating out of 5 or a descriptive rating like "Excellent", "Good", etc.
        public ICollection<CarDto>? Cars { get; set; }

    }
}
