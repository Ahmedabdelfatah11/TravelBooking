
using System.Text.Json.Serialization;
using TravelBooking.Core.Models;

namespace TravelBooking.APIs.DTOS.Booking
{ 
    public class BookingDto
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public BookingType BookingType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? TotalPrice { get; set; }
        public object? AgencyDetails { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }

        public string? PaymentStatus { get; set; } 
    }

}
