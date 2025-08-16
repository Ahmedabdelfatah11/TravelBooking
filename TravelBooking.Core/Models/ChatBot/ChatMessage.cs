using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelBooking.Models;

namespace TravelBooking.Core.Models.ChatBot
    {

    public enum ChatMessageType
    {
        General,
        BookingInquiry,
        HotelSearch,
        FlightSearch,
        CarRental,
        TourSearch,
        Support
    }
    public class ChatMessage : BaseEntity
    {
        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        public string Response { get; set; } = string.Empty;

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public string SessionId { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool IsFromUser { get; set; } = true;

        public string? Context { get; set; } // JSON string of relevant data

        public ChatMessageType MessageType { get; set; } = ChatMessageType.General;
    }
    }


