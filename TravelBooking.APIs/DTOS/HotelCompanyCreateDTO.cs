


namespace TravelBooking.Core.DTOS
{
    public class HotelCompanyCreateDTO
    {

        [Required]
        [UniqueNameAttribute]
        public string Name { get; set; }

        [Required]
        [MinLength(10)]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [ValidImageUrlAttribute]
        public string ImageUrl { get; set; }

    
        [RatingRangeStringAttribute] // custom validation
        public string Rating { get; set; }
    }
}
