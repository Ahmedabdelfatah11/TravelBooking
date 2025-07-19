using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public class TourCompany : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Location { get; set; } // Location of the tour company, e.g., "Rome, Italy"
        public string? rating { get; set; } // Rating out of 5 or a descriptive rating like "Excellent", "Good", etc.

        public ICollection<Tour>? Tours { get; set; } = new List<Tour>(); 
    }
}