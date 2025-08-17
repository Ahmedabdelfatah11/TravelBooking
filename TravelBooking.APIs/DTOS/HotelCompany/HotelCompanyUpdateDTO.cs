namespace TravelBooking.APIs.DTOS.HotelCompany
{
    public class HotelCompanyUpdateDTO
    {

        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [UniqueName]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }
         
        public IFormFile? Image { get; set; }

        [Required]
        [RatingRangeString]
        public string Rating { get; set; }
    }
}
