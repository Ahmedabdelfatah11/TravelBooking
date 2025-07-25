using TravelBooking.Core.Models;

namespace TravelBooking.APIs.DTOS.Booking.TourBooking
{
    public class TourBookingResultDto
    {
        public int BookingId { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public string TourName { get; set; }
        public string? Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TourCategory? Category { get; set; }
        public int TourId { get; set; }
    }
}
