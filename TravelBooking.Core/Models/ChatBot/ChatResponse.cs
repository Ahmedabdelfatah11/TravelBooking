namespace TravelBooking.Core.Models.ChatBot
{
    public class ChatResponse
    {
        public string Response { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public List<SuggestionButton>? Suggestions { get; set; }
        public object? Data { get; set; } // Additional data like search results
        public bool IsTyping { get; set; } = false;
    }
}
