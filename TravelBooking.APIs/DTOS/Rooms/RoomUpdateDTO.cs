using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TravelBooking.APIs.DTOS.Rooms
{
    public class RoomUpdateDTO
    {
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public bool? IsAvailable { get; set; }

        public string? RoomType { get; set; }

        public int? HotelCompanyId { get; set; }

        [StringLength(500, MinimumLength = 10)]
        public string? Description { get; set; }

        [MaxImagesCount(3, ErrorMessage = "You can only upload up to 3 images per room")]
        public List<IFormFile>? RoomImages { get; set; } = new();
    }
}