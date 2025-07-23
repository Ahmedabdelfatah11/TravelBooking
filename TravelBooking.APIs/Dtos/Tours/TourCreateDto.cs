using TravelBooking.Core.Models;

namespace TravelBooking.APIs.Dtos.Tours
{
    public class TourCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required(ErrorMessage = "StartDate is required.")]
        [FutureDate(ErrorMessage = "StartDate  must be in the future.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "EndDate is required.")]
        [GreaterThan("StartDate", ErrorMessage = "EndDate time must be after StartDate time.")]
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public string? Destination { get; set; }
        [Required(ErrorMessage = "MaxGuests is required.")]

        public int MaxGuests { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
        public TourCategory? Category { get; set; }
        [Required(ErrorMessage = "TourCompanyId is required.")]
        public int? TourCompanyId { get; set; }
    }
}
