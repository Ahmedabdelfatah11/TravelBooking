
namespace TravelBooking.APIs.DTOS.FlightCompany
{
    public class FlightCompanyDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public int? FlightCount { get; set; }

        [Required]
        public string? AdminId { get; set; }
    }
}
