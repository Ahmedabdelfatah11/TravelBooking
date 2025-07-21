﻿using TravelBooking.Core.Validations;

namespace TravelBooking.APIs.Dtos.Rooms
{
    public class RoomToReturnDTO
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public string RoomType { get; set; }
        public string Description { get; set; }
        public int HotelCompanyId { get; set; }
        public string HotelCompanyName { get; set; }

        public List<RoomImageReadDTO> RoomImages { get; set; } = new();

    }
}
