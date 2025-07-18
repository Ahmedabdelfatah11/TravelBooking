namespace TravelBooking.APIs.DTOS
{
    public class HotelCompanyUpdateDTO
    {

        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [UniqueNameAttribute]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [ValidImageUrlAttribute]
        public string ImageUrl { get; set; }

        [Required]
        [RatingRangeStringAttribute]
        public string Rating { get; set; }
        [Required]
        public int HotelCompanyId { get; set; }
    }
}
