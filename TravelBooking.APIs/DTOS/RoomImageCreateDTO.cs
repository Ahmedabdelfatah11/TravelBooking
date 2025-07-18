namespace TravelBooking.APIs.DTOS
{
    public class RoomImageCreateDTO
    {
        [Required]
        [ValidImageUrlAttribute]
        public string ImageUrl { get; set; }
    }
}
