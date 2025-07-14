using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public class Car:BaseEntity
    {
        public String Model { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public string Location {get; set; } // Location where the car is available for rent 
        public string ImageUrl { get; set; }
        public int Capacity { get; set; } // Number of passengers the car can accommodate
        [ForeignKey("CarRentalCompany")]
        public int? CompanyID {  get; set; }
        public CarRentalCompany? CarRentalCompany { get; set; }
        public ICollection<Booking>? bookings { get; set; }


    }
}
