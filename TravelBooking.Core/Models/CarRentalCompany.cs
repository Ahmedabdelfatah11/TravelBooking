using System;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.ComponentModel.DataAnnotations.Schema;
>>>>>>> 5058d87932f09652166bfc11440e8e2ddc8faf2e
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public class CarRentalCompany:BaseEntity
    {
        public string Name { get; set; } // Name of the car rental company
        public string description { get; set; } // Description of the car rental company
        public string Location { get; set; } // Location of the car rental company, e.g., "Los Angeles, USA"
        public string ImageUrl { get; set; } // URL to the company's logo or image
        public string Rating { get; set; } // Rating out of 5 or a descriptive rating like "Excellent", "Good", etc.
<<<<<<< HEAD
        public ICollection<Car> Cars { get; set; } // Navigation property for related cars
=======
        

        public ICollection<Car>? Cars { get; set; }
>>>>>>> 5058d87932f09652166bfc11440e8e2ddc8faf2e

    }
}
