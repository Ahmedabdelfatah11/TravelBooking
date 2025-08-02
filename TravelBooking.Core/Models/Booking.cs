using System;
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
        public SeatClass? SeatClass { get; set; }
          
        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        public int? CarId { get; set; }
        public Car? Car { get; set; }

        public int? FlightId { get; set; }
        public Flight? Flight { get; set; }

        public int? TourId { get; set; }
        public Tour? Tour { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; } 

        public Payment? Payment { get; set; }

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }  
        [NotMapped]
        public decimal TotalPrice
        {
            get
            {
                int duration = (int)(EndDate - StartDate).TotalDays;
                if (duration <= 0) duration = 1;

                decimal total = 0;

                switch (BookingType)
                {
                    case BookingType.Room:
                        total = (Room?.Price ?? 0) * duration;
                        break;

                    case BookingType.Car:
                        total = (Car?.Price ?? 0) * duration;
                        break;

                    case BookingType.Flight:
                        total = Flight?.GetPrice(SeatClass) ?? 0;
                        break;

                    case BookingType.Tour:
                        total = Tour?.Price ?? 0;
                        break;

                    default:
                        total = 0;
                        break;
                }

                return total;
            }
        } 
    }
}
