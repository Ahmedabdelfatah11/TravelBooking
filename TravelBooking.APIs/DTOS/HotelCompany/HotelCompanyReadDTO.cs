using TravelBooking.APIs.DTOS.Rooms;

namespace TravelBooking.APIs.DTOS.HotelCompany
{
    public class HotelCompanyReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public string Rating { get; set; }
        public string? AdminId { get; set; } 

        public List<RoomToReturnDTO> Rooms { get; set; } = new();
    }
}
