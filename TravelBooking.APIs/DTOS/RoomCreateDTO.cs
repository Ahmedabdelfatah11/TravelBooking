using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.DTOS
{
    public class RoomCreateDTO
    {
        [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative")]
        public decimal Price { get; set; }

        [Required]
        public bool IsAvailable { get; set; } = true;
        [Required]
        public string Description { get; set; }

        [Required]
        public string RoomType { get; set; } // "Single", "Double", "Suite"


        [Required]
        public int HotelCompanyId { get; set; }

        [MaxImagesCount(3, ErrorMessage = "You can only upload up to 3 images per room")]
        public List<RoomImageCreateDTO> RoomImages { get; set; } = new();
    }
}
