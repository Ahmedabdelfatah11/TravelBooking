using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public class Car : BaseEntity
    {
        public string Model { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public string Location { get; set; } // Location where the car is available for rent 
        public string ImageUrl { get; set; }
        public int Capacity { get; set; } // Number of passengers the car can accommodate
        [ForeignKey("RentalCompany")]
        public int? RentalCompanyId { get; set; } // Foreign key to CarRentalCompany
        public CarRentalCompany? RentalCompany { get; set; } // Navigation property to CarRentalCompany

    }
}
