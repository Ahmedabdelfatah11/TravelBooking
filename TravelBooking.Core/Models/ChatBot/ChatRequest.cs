
namespace TravelBooking.Core.Models.ChatBot
{
    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
        public string? SessionId { get; set; }
        public string? UserId { get; set; }
        public ChatMessageType MessageType { get; set; } = ChatMessageType.General;
    }
}
