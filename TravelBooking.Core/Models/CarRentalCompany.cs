using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public class CarRentalCompany : BaseEntity
    {
        public string Name { get; set; } // Name of the car rental company
        public string description { get; set; } // Description of the car rental company
        public string Location { get; set; } // Location of the car rental company, e.g., "Los Angeles, USA"
        public string ImageUrl { get; set; } // URL to the company's logo or image
        public string Rating { get; set; } // Rating out of 5 or a descriptive rating like "Excellent", "Good", etc.
        public ICollection<Car>? Cars { get; set; } // Navigation property for related cars

    }
}
