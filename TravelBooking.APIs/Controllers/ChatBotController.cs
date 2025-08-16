using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TravelBooking.Core.Models.ChatBot;
using TravelBooking.Repository.Hubs;
using TravelBooking.Service.Services.ChatBot;

namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ChatBotController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatBotController(IChatBotService chatBotService, IHubContext<ChatHub> hubContext)
        {
            _chatBotService = chatBotService;
            _hubContext = hubContext;
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            try
            {
                // Get user ID from claims if authenticated
                var userId = User.FindFirst("uid")?.Value;
                request.UserId = userId;

                // Send typing indicator
                if (!string.IsNullOrEmpty(request.SessionId))
                {
                    await _hubContext.Clients.Group(request.SessionId)
                        .SendAsync("ReceiveTypingIndicator", true);
                }

                // Process the message
                var response = await _chatBotService.ProcessMessageAsync(request);

                // Stop typing indicator
                if (!string.IsNullOrEmpty(request.SessionId))
                {
                    await _hubContext.Clients.Group(request.SessionId)
                        .SendAsync("ReceiveTypingIndicator", false);

                    // Send response via SignalR
                    await _hubContext.Clients.Group(request.SessionId)
                        .SendAsync("ReceiveMessage", response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your message." });
            }
        }

        [HttpGet("history/{sessionId}")]
        public async Task<IActionResult> GetChatHistory(string sessionId)
        {
            try
            {
                var history = await _chatBotService.GetChatHistoryAsync(sessionId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving chat history." });
            }
        }

        [HttpPost("classify-message")]
        public IActionResult ClassifyMessage([FromBody] string message)
        {
            try
            {
                var messageType = ClassifyMessageType(message.ToLower());
                return Ok(new { messageType });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while classifying the message." });
            }
        }

        private ChatMessageType ClassifyMessageType(string message)
        {
            // Simple keyword-based classification
            if (message.Contains("hotel") || message.Contains("room") || message.Contains("accommodation"))
                return ChatMessageType.HotelSearch;

            if (message.Contains("flight") || message.Contains("airline") || message.Contains("fly"))
                return ChatMessageType.FlightSearch;

            if (message.Contains("car") || message.Contains("rental") || message.Contains("drive"))
                return ChatMessageType.CarRental;

            if (message.Contains("tour") || message.Contains("trip") || message.Contains("excursion"))
                return ChatMessageType.TourSearch;

            if (message.Contains("booking") || message.Contains("reservation") || message.Contains("cancel"))
                return ChatMessageType.BookingInquiry;

            if (message.Contains("help") || message.Contains("support") || message.Contains("problem"))
                return ChatMessageType.Support;

            return ChatMessageType.General;
        }
    }
}

