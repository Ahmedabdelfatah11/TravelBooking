namespace TravelBooking.APIs.DTOS.Rooms
{
    public class RoomImageCreateDTO
    {
        [Required]
        [ValidImageUrl]
        public string ImageUrl { get; set; }
    }
}
