namespace TravelBooking.APIs.Dtos.HotelCompany
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

        [Required]
        [ValidImageUrl]
        public string ImageUrl { get; set; }

        [Required]
        [RatingRangeString]
        public string Rating { get; set; }
        [Required]
        public int HotelCompanyId { get; set; }
    }
}
