using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Models;

namespace TravelBooking.Core.Models
{
    public class Reviews: BaseEntity
    {
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("HotelCompany")]
        public int? HotelCompanyId { get; set; }
        [ForeignKey("FlightCompany")]
        public int? FlightCompanyId { get; set; }
        [ForeignKey("CarRentalCompany")]
        public int? CarRentalCompanyId { get; set; }
        [ForeignKey("TourCompany")]
        public int? TourCompanyId { get; set; }
        [Required]
        [StringLength(50)]
        public string CompanyType { get; set; } // "Hotel", "Flight", "CarRental", "Tour"
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }
        [MaxLength(1000)]
        public string Comment { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
        // Navigation Properties
        public virtual ApplicationUser User { get; set; }
        public virtual HotelCompany HotelCompany { get; set; }
        public virtual FlightCompany FlightCompany { get; set; }
        public virtual CarRentalCompany CarRentalCompany { get; set; }
        public virtual TourCompany TourCompany { get; set; }

    }
}
