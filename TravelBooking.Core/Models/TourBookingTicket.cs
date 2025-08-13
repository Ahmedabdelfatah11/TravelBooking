namespace TravelBooking.Core.Models
{
    public class TourBookingTicket
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public int TicketId { get; set; }
        public TourTicket Ticket { get; set; }

        public int Quantity { get; set; }
        public bool IsIssued { get; set; } = false;
    }
}