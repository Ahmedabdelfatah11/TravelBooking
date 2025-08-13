using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        [JsonPropertyName("RoomType")]
        public RoomType RoomType { get; set; }

        public string Description { get; set; }
        [Required(ErrorMessage = "Departure time is required.")]
        [FutureDate(ErrorMessage = "Departure time must be in the future.")]
        public DateTime From { get; set; }

        [Required(ErrorMessage = "Arrival time is required.")]
        [GreaterThan("DepartureTime", ErrorMessage = "Arrival time must be after departure time.")]
        public DateTime To { get; set; }
        public float[]? Embedding { get; set; }

        [ForeignKey("HotelId")]
        public int? HotelId { get; set; }
        public HotelCompany? Hotel { get; set; }
        public ICollection<RoomImage> Images { get; set; } = new List<RoomImage>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
