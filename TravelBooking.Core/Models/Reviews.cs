using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Models;

namespace TravelBooking.Core.Models
{
    public class Reviews: BaseEntity
    {
        public decimal Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //public int UserId { get; set; }
        //public ApplicationUser? User { get; set; }
        //public int BookingId { get; set; }
        //public Booking? Booking { get; set; }
        //public int FlightId { get; set; }
        //public Flight? Flight { get; set; }
        //public int HotelId { get; set; }
        //public Hotel? Hotel { get; set; }
        //public int CarRentalId { get; set; }
        //public CarRental? CarRental { get; set; }
        //public int TourId { get; set; }
        //public Tour? Tour { get; set; }

    }
}
