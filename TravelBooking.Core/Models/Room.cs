using System;
using System.Collections.Generic;
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
    public class Room: BaseEntity
    {
        public string RoomType { get; set; } // Type of room (e.g., Single, Double, Suite)
        public decimal Price { get; set; } // Price per night for the room
        public bool IsAvailable { get; set; } // Availability status of the room
        public RoomType roomType { get; set; } // Enum for room type
    } 
}
