using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public enum RoomType
    {
        Single,
        Double,
        Suite
    }
    public class Room: BaseEntity
    {
        
        public decimal Price { get; set; } // Price per night for the room
        public bool IsAvailable { get; set; } // Availability status of the room
        public RoomType RoomType { get; set; } // Enum for room type
<<<<<<< HEAD
        [ForeignKey("HotelCompany")]
        public int HotelId { get; set; } // Foreign key to HotelCompany
        public HotelCompany Hotel { get; set; } // Navigation property to HotelCompany
=======
        [ForeignKey("Hotel")]
        public int? HotelId {  get; set; }
        public HotelCompany? Hotel  { get; set; }
        public ICollection<Booking>? Bookings  { get; set; }

>>>>>>> 5058d87932f09652166bfc11440e8e2ddc8faf2e
    } 
}
