using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Models;

namespace TravelBooking.Core.Models
{
    public class Favoritet:BaseEntity
    {
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("HotelCompany")]
        public int? HotelCompanyId { get; set; }
        [ForeignKey("Flight")]
        public int? FlightCompanyId { get; set; }
        [ForeignKey("CarRentalCompany")]
        public int? CarRentalCompanyId { get; set; }
        [ForeignKey("TourCompany")]
        public int? TourCompanyId { get; set; }
        [Required]
        [StringLength(50)]
        public string CompanyType { get; set; } = string.Empty;// "Hotel", "Flight", "CarRental", "Tour"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.Date;
        public virtual ApplicationUser User { get; set; }
        public virtual HotelCompany HotelCompany { get; set; }
        public virtual Flight Flight { get; set; }

        public virtual CarRentalCompany CarRentalCompany { get; set; }
        public virtual TourCompany TourCompany { get; set; }
    }
}
