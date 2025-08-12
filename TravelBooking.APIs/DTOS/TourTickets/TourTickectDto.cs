namespace TravelBooking.APIs.DTOS.TourTickets
{
    public class TourTicketDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int AvailableQuantity { get; set; }
        public bool IsActive { get; set; }
        public int TourId { get; set; }
    }

}
