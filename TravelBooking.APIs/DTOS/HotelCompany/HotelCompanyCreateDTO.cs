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

        [Required]
        [ValidImageUrl]
        public string ImageUrl { get; set; }


        [RatingRangeString] // custom validation
        public string Rating { get; set; }

       
        public string? AdminId { get; set; }
    }
}
