namespace TravelBooking.APIs.DTOS.Booking.FlightBooking
{
    public class FlightBookingResultDto
    {
        public int BookingId { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public int FlightId { get; set; }
    }

}
