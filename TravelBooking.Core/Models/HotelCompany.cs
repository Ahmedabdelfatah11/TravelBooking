using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Models;

namespace TravelBooking.Core.Models
{
    public class HotelCompany : BaseEntity
    {

        public string Name { get; set; }
        public string Description { get; set; } // Description of the hotel company
        public string Location { get; set; } // Location of the hotel company, e.g., "Paris, France"
        public string ImageUrl { get; set; }
        public string Rating { get; set; }
        public ICollection<Room>? Rooms { get; set; } // Navigation property for related rooms
       
        public ICollection<Favoritet> favoritets { get; set; } = new List<Favoritet>();

        public ICollection<Review> reviews { get; set; } = new List<Review>();

        // New Relationship with ApplicationUser (HotelAdmin)
        public string? AdminId { get; set; } // FK to AspNetUsers
        public ApplicationUser? Admin { get; set; } // Navigation Property

    }
}
