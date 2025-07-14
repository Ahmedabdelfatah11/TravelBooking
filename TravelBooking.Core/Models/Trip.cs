using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public class Trip : BaseEntity
    {

        public DateTime StartDate { get; set; } // Start date of the trip
        public DateTime EndDate { get; set; } // End date of the trip
        public string Description { get; set; } // Description of the trip
        public string Destination { get; set; } // Destination of the trip (e.g., city or country)
        public decimal Price { get; set; } // Total price of the trip 
        [ForeignKey("TourCompany")]
        public int? TourCompanyId { get; set; } // Foreign key for the associated tour company
        public TourCompany? TourCompany { get; set; } // Navigation property for the tour company
    }
}
