
using TravelBooking.Core.Models.ChatBot;

namespace TravelBooking.Service.Services.ChatBot
{
    public interface IChatBotService
    {
         Task<ChatResponse> ProcessMessageAsync(ChatRequest request);
        Task<List<ChatMessage>> GetChatHistoryAsync(string sessionId);
        Task SaveChatMessageAsync(ChatMessage message);
    }
}
