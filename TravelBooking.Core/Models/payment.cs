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
<<<<<<< HEAD
        [ForeignKey("Booking")] 
        public int BookingId { get; set; } // Foreign key to Booking
        public Booking Booking { get; set; } // Navigation property to Booking
=======

        [ForeignKey("Booking")]
        public int? BookId { get; set; }
        public Booking? Booking { get; set; }
>>>>>>> 5058d87932f09652166bfc11440e8e2ddc8faf2e
    }
}
