using System; 

namespace TravelBooking.Core.Models
{
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed,
    }
    public class Payment : BaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public PaymentStatus PaymentStatus { get; set; }
        // Navigation
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
