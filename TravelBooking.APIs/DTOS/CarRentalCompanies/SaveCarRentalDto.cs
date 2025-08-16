using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.DTOS.CarRentalCompanies
{
    public class SaveCarRentalDto
    {
        //public int Id { get; set; }
        public string? Name { get; set; } // Name of the car rental company
        public string? Description { get; set; } // Description of the car rental company
        public string? Location { get; set; } // Location of the car rental company, e.g., "Los Angeles, USA"
        public IFormFile? Image { get; set; }
        public string? Rating { get; set; } // Rating out of 5 or a descriptive rating like "Excellent", "Good", etc.
       
        public string? AdminId { get; set; }
    }
}
