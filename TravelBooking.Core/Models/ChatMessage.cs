using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelBooking.Models;

namespace TravelBooking.Core.Models
    {
        public class ChatMessage
        {
            public int Id { get; set; }

            [Required]
            public string UserInput { get; set; } = "";

            [Required]
            public string GeminiResponse { get; set; } = "";

            [Required]
            public string UserId { get; set; } = "";

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public bool IsUserMessage { get; set; } = true;

            public string MessageType { get; set; } = "text";

            [ForeignKey("UserId")]
            public ApplicationUser? User { get; set; }
        }
    }


