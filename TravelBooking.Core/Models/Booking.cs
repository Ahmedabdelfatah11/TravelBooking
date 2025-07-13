using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public enum Status
    {
        Pending,
        Confirmed,
        Cancelled
    }
    public enum BookingType
    {
        Hotel,
        CarRental,
        Flight,
        Tour
    }
    public class Booking:BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Status status { get; set; }
        public BookingType BookingType { get; set; }
        public int ReferenceId { get; set; } // Could be HotelId, CarId, etc. based on BookingType  


    }
}
