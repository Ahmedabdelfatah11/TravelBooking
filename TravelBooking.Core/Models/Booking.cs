using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using TravelBooking.Models;

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
        Room,
        Car,
        Flight,
        Tour
    }
    public class Booking : BaseEntity

    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Status Status { get; set; }
        public BookingType BookingType { get; set; }

        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        public int? CarId { get; set; }
        public Car? Car { get; set; }

        public int? FlightId { get; set; }
        public Flight? Flight { get; set; }
        public Payment? Payment { get; set; }

        public int? TourId { get; set; }
        public Tour? Tour { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
