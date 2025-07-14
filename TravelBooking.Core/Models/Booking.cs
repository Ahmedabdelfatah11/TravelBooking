<<<<<<< HEAD
﻿using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.ComponentModel.DataAnnotations.Schema; 
=======
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> 5058d87932f09652166bfc11440e8e2ddc8faf2e

namespace TravelBooking.Core.Models
{
    public enum Status
    {
        Pending,
        Confirmed,
        Cancelled
    }
<<<<<<< HEAD

=======
>>>>>>> 5058d87932f09652166bfc11440e8e2ddc8faf2e
    public enum BookingType
    {
        Hotel,
        CarRental,
        Flight,
        Tour
<<<<<<< HEAD
    } 
    public class Booking:BaseEntity

    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Status status { get; set; }
        public BookingType BookingType { get; set; }

        public int? RefId { get; set; }
        public int? PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; } 
    }
=======
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

>>>>>>> 5058d87932f09652166bfc11440e8e2ddc8faf2e
}
