namespace TravelBooking.APIs.DTOS.FlightCompany
{
    public class FlightCompanyCreateDto
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public string? Location { get; set; } 
        public int Rating { get; set; }
        [Required]
        public string AdminId { get; set; }
    }
}
