using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Core.Models
{
    public enum RoomType
    {
        Single,
        Double,
        Suite
    }
    public class Room : BaseEntity
    {

        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public RoomType RoomType { get; set; }

        public string Description { get; set; }

        [ForeignKey("HotelCompany")]
        public int? HotelId { get; set; }
        public HotelCompany? Hotel { get; set; }
        public ICollection<RoomImage> Images { get; set; } = new List<RoomImage>();
    }
}
