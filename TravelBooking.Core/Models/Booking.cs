using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
    public class Booking : BaseEntity
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Status Status { get; set; }
        public BookingType BookingType { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Room")]
        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        [ForeignKey("Car")]
        public int? CarId { get; set; }
        public Car? Car { get; set; }

        [ForeignKey("Flight")]
        public int? FlightId { get; set; }
        public Flight? Flight { get; set; }

        [ForeignKey("Trip")]
        public int? TripId { get; set; }
        public Trip? Trip { get; set; }

    }

}
