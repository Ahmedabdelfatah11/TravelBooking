using TravelBooking.Core.Models;

namespace TravelBooking.APIs.DTOS.Rooms
{
    public class RoomImageReadDTO : BaseEntity
    {
        public string ImageUrl { get; set; }

        public int RoomId { get; set; }
        //public Room Room { get; set; }

    }
}
