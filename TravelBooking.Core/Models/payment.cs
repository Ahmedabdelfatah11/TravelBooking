using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public class Payment : BaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } // Method of payment (e.g., Credit Card, PayPal)
        public string TransactionId { get; set; } // Unique transaction identifier
        public Status PaymentStatus { get; set; } // Status of the payment (e.g., Pending, Completed, Failed)
        [ForeignKey("Booking")]
        public int BookingId { get; set; } // Foreign key to Booking
        public Booking Booking { get; set; } // Navigation property to Booking
    }
}
