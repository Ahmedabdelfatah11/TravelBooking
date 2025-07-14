using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public enum TripCategory
    {
        Adventure,
        Relaxation,
        Cultural,
        Nature,
        Historical
    }
    public class TourCompany:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; } // Location of the tour company, e.g., "Rome, Italy"
        public string rating { get; set; } // Rating out of 5 or a descriptive rating like "Excellent", "Good", etc.
        public string ImageUrl { get; set; } // URL to the company's logo or image

        public ICollection<Trip>? Trips { get; set; }
    }
}