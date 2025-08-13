namespace TravelBooking.APIs.DTOS.Booking.TourBooking
{
    public class TourBookingDto
    {
        [Required]
        public List<TourTicketSelectionDto> Tickets { get; set; }
    }

    public class TourTicketSelectionDto
    {
        [Required]
        public string Type { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

}
