using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public class HotelCompany : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; } // Description of the hotel company
        public string Location { get; set; } // Location of the hotel company, e.g., "Paris, France"
        public int Rating { get; set; }
        public ICollection<Room> Rooms { get; set; } // Navigation property for related rooms

    }
}
