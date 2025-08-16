namespace TravelBooking.Core.Models.ChatBot
{
    public class SuggestionButton
    {
        public string Text { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
