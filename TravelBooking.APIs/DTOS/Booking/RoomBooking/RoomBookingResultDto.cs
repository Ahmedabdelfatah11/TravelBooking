using TravelBooking.Core.Models;

namespace TravelBooking.APIs.DTOS.Booking.RoomBooking
{
    public class RoomBookingResultDto
    {
        public int BookingId { get; set; }
        public Status Status { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RoomId { get; set; }
        public string RoomType { get; set; }
        public decimal TotalPrice { get; set; }

        
    }

}
