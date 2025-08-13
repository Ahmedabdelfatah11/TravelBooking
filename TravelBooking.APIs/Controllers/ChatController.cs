using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelBooking.Service.Services;
using TravelBooking.Core.Models;
using Google.Cloud.Location;

namespace TravelBooking.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;
        private readonly ChatHistoryService _historyService;

        public ChatController(ChatService chatService, ChatHistoryService historyService)
        {
            _chatService = chatService;
            _historyService = historyService;
        }

        /// <summary>
        /// Send a message to the AI travel assistant
        /// </summary>
        [HttpPost("send")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageRequest request)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User authentication required.");

            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Message cannot be empty.");

            try
            {
                var response = await _chatService.GetResponseAsync(request.Message, userId);

                return Ok(new ChatMessageResponse
                {
                    Message = response.Message,
                    QueryType = response.QueryType.ToString(),
                   
                    RecommendedServices = new
                    {
                        Cars = response.RecommendedServices?.Cars?.Select(c => new
                        {
                            c.Id,
                            c.Model,
                            c.Price,
                            c.Location,
                            RentalCompany = c.RentalCompany?.Name
                        }),
                        Rooms = response.RecommendedServices?.Rooms?.Select(r => new
                        {
                            r.Id,
                            r.RoomType,
                            r.Price,
                            Hotel = r.Hotel?.Name,
                            Address = r.Hotel?.Location
                        }),
                        Flights = response.RecommendedServices?.Flights?.Select(f => new
                        {
                            f.Id,
                            f.DepartureAirport,
                            f.ArrivalAirport,
                            f.EconomyPrice,
                            Company = f.FlightCompany?.Name
                        }),
                        Tours = response.RecommendedServices?.Tours?.Select(t => new
                        {
                            t.Id,
                            t.Name,
                            t.Price,
                            Company = t.TourCompany?.Name
                        })
                    },
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "حدث خطأ في معالجة الرسالة. يرجى المحاولة مرة أخرى." });
            }
        }

        /// <summary>
        /// Get user's chat history
        /// </summary>
        [HttpGet("history")]
        [Authorize(Roles = "User")]
        public IActionResult GetHistory([FromQuery] int count = 10)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var history = _historyService.GetLatestMessages(userId, count);

            return Ok(history.Select(h => new ChatHistoryItem
            {
                Id = h.Id,
                UserMessage = h.UserInput,
                AssistantResponse = h.GeminiResponse,
                Timestamp = h.CreatedAt
            }).OrderBy(h => h.Timestamp));
        }

        /// <summary>
        /// Clear user's chat history
        /// </summary>
        [HttpDelete("clear")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ClearHistory()
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _historyService.ClearHistoryAsync(userId);
            return Ok(new { message = "تم مسح تاريخ المحادثات بنجاح." });
        }

        /// <summary>
        /// Get AI suggestions for travel planning
        /// </summary>
        [HttpPost("suggestions")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetSuggestions([FromBody] TravelPreferencesRequest request)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var prompt = BuildSuggestionPrompt(request);
            var response = await _chatService.GetResponseAsync(prompt, userId);

            return Ok(new
            {
                suggestions = response.Message,
                recommendedServices = response.RecommendedServices
            });
        }

        /// <summary>
        /// Get quick travel tips
        /// </summary>
        [HttpGet("tips")]
        [AllowAnonymous]
        public IActionResult GetTravelTips()
        {
            var tips = new[]
            {
                "احجز رحلتك مبكرًا للحصول على أفضل الأسعار",
                "تأكد من صحة جواز السفر قبل السفر بـ 6 أشهر على الأقل",
                "احتفظ بنسخ من وثائقك المهمة في مكان منفصل",
                "تعرف على العادات المحلية للبلد التي تزورها",
                "احمل معك دواء الإسعافات الأولية الأساسي",
                "تحقق من أحوال الطقس قبل السفر"
            };

            var randomTip = tips[new Random().Next(tips.Length)];

            return Ok(new { tip = randomTip });
        }

        

        
        private string BuildSuggestionPrompt(TravelPreferencesRequest request)
        {
            return $@"
                اقترح لي خيارات سفر مناسبة بناءً على التفضيلات التالية:
                - الوجهة المفضلة: {request.PreferredDestination ?? "أي مكان في مصر"}
                - الميزانية: {request.Budget} جنيه
                - تاريخ السفر: {request.TravelDate:yyyy-MM-dd}
                - عدد المسافرين: {request.NumberOfTravelers}
                - نوع الإقامة المفضل: {request.PreferredAccommodation}
                - الأنشطة المفضلة: {string.Join(", ", request.PreferredActivities)}
            ";
        }
    }

    // DTOs for requests and responses
    public class ChatMessageRequest
    {
        public string Message { get; set; }
    }

    public class ChatMessageResponse
    {
        public string Message { get; set; }
        public string QueryType { get; set; }
        public List<string> SuggestedActions { get; set; }
        public object RecommendedServices { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class ChatHistoryItem
    {
        public int Id { get; set; }
        public string UserMessage { get; set; }
        public string AssistantResponse { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class TravelPreferencesRequest
    {
        public string? PreferredDestination { get; set; }
        public decimal? Budget { get; set; }
        public DateTime TravelDate { get; set; }
        public int NumberOfTravelers { get; set; }
        public string? PreferredAccommodation { get; set; }
        public List<string> PreferredActivities { get; set; } = new();
    }

    public class ItineraryRequest
    {
        public string Destination { get; set; }
        public int Days { get; set; }
        public decimal Budget { get; set; }
        public int Travelers { get; set; }
        public string TripType { get; set; } // Cultural, Adventure, Relaxation, etc.
        public List<string> Interests { get; set; } = new();
    }
}