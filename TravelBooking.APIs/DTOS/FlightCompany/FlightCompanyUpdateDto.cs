namespace TravelBooking.APIs.DTOS.FlightCompany
{
    public class FlightCompanyUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public string? Location { get; set; }
    }
}
