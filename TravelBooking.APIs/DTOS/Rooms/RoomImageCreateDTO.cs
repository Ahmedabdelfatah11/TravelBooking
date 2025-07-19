namespace TravelBooking.APIs.Dtos.Rooms
{
    public class RoomImageCreateDTO
    {
        [Required]
        [ValidImageUrl]
        public string ImageUrl { get; set; }
    }
}
