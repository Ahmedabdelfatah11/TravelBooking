using Microsoft.EntityFrameworkCore;
using TravelBooking.Core.Models;
using TravelBooking.Repository.Data;

namespace TravelBooking.Service.Services
{
    public class ChatHistoryService
    {
        private readonly AppDbContext _context;

        public ChatHistoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveMessageAsync(string userInput, string response, string userId)
        {
            var message = new ChatMessage
            {
                UserInput = userInput,
                GeminiResponse = response,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
        }

        public List<ChatMessage> GetLatestMessages(string userId, int count = 10)
        {
            return _context.ChatMessages
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(count)
                .ToList();
        }

        public async Task ClearHistoryAsync(string userId)
        {
            var messages = _context.ChatMessages.Where(m => m.UserId == userId);
            _context.ChatMessages.RemoveRange(messages);
            await _context.SaveChangesAsync();
        }
    }
}