using TravelBooking.APIs.DTOS.TourTickets;
using TravelBooking.Core.Models;

namespace TravelBooking.APIs.DTOS.Tours
{
    public class TourCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "StartDate is required.")]
        [FutureDate(ErrorMessage = "StartDate must be in the future.")]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "EndDate is required.")]
        [GreaterThan("StartDate", ErrorMessage = "EndDate must be after StartDate.")]
        public DateTime EndDate { get; set; }

        public string? Description { get; set; }
        public string? Destination { get; set; }

        [Required(ErrorMessage = "MaxGuests is required.")]
        public int MaxGuests { get; set; }

        public int MinGroupSize { get; set; }
        public int MaxGroupSize { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        [EnumDataType(typeof(TourCategory), ErrorMessage = "Invalid category value.")]
        public TourCategory? Category { get; set; }

        public string Languages { get; set; }

        [Required(ErrorMessage = "TourCompanyId is required.")]
        public int? TourCompanyId { get; set; }
        public List<string> ImageUrls { get; set; } = new();

        public List<TourTicketCreateDto>? Tickets { get; set; }
        public List<string> IncludedItems { get; set; } = new();
        public List<string> ExcludedItems { get; set; } = new();
        public List<TourQuestionDto> Questions { get; set; } = new();
    }

}
