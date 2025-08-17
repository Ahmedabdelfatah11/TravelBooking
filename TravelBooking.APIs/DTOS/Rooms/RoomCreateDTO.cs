using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.APIs.DTOS.Rooms
{
    public class RoomCreateDTO
    {
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public bool IsAvailable { get; set; } = true;

        [Required]
        public string Description { get; set; }

        [Required]
        public string RoomType { get; set; } // "Single", "Double", "Suite"

        [Required]
        public int HotelCompanyId { get; set; }

        [Required]
        [FutureDate(ErrorMessage = "Start date must be in the future")]
        public DateTime From { get; set; }

        [Required]
        [GreaterThan("From", ErrorMessage = "End date must be after start date")]
        public DateTime To { get; set; }

        [MaxImagesCount(3, ErrorMessage = "You can only upload up to 3 images per room")]
        [FromForm]
        public List<IFormFile> RoomImages { get; set; } = new();

    }
}
