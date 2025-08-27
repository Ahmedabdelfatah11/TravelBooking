namespace TravelBooking.APIs.DTOS.TourTickets
{
    public class TourTicketCreateDto
    {
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required]
        public int TourId { get; set; }

        [Required]
        public int AvailableQuantity { get; set; }

        public bool IsActive { get; set; } = true;
    }

}
