namespace TravelBooking.APIs.DTOS.Rooms
{
    public class RoomUpdateDTO
    {

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        [Required]
        public string RoomType { get; set; } //
        public int HotelCompanyId { get; set; }
        [MaxImagesCount(3, ErrorMessage = "You can only upload up to 3 images per room")]

        public List<RoomImageCreateDTO> RoomImages { get; set; } = new();
    }
}
