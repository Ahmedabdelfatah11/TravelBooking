using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TravelBooking.Core.Models
{
    public class TourTicket : BaseEntity
    {
        public string Type { get; set; } = "Adult"; // or "Child", "VIP", etc.

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int TourId { get; set; }

        [ForeignKey("TourId")]
        public Tour Tour { get; set; }

        public int AvailableQuantity { get; set; }

        public bool IsActive { get; set; } = true;
    }
}