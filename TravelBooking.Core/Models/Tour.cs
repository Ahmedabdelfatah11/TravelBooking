using Org.BouncyCastle.Crypto.Signers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public enum TourCategory
    {
        Adventure,
        Relaxation,
        Cultural,
        Nature,
        Historical
    }
    public class Tour : BaseEntity
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; } =DateTime.UtcNow;
        public DateTime EndDate { get; set; } 
        public string? Description { get; set; } 
        public string? Destination { get; set; } // Destination of the Tour (e.g., city or country)
        public int MaxGuests { get; set; } // Maximum allowed guests for the Tour

        public decimal Price { get; set; } // Total price of the Tour 
        public TourCategory? Category { get; set; }
        [ForeignKey("TourCompany")]
        public int? TourCompanyId { get; set; } // Foreign key for the associated tour company

        public TourCompany? TourCompany { get; set; } // Navigation property for the tour company

        public ICollection<TourImage> TourImages { get; set; } = new List<TourImage>();
    }
}
