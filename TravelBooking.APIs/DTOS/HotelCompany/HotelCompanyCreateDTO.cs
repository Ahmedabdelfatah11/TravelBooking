namespace TravelBooking.APIs.DTOS.HotelCompany
{
    public class HotelCompanyCreateDTO
    {

        [Required]
        [UniqueName]
        public string Name { get; set; }

        [Required]
        [MinLength(10)]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }
         
        [Required(ErrorMessage = "Image file is required.")]
        public IFormFile Image { get; set; }

        [RatingRangeString]
        public string Rating { get; set; }

        public string? AdminId { get; set; }
    }
}
