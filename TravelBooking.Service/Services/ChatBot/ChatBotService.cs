using Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models.ChatBot;
using TravelBooking.Repository.Data;

namespace TravelBooking.Service.Services.ChatBot
{
    public class ChatBotService : IChatBotService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _geminiApiKey;
        private readonly string _geminiApiUrl;

        public ChatBotService(AppDbContext context, HttpClient httpClient, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _geminiApiKey = configuration["Gemini:ApiKey"] ?? "AIzaSyByK3FcpimqqXKJfIThkZeA56QgoVcWniE";
            _geminiApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";
        }

        public async Task<ChatResponse> ProcessMessageAsync(ChatRequest request)
        {
            try
            {
                // Generate session ID if not provided
                if (string.IsNullOrEmpty(request.SessionId))
                {
                    request.SessionId = Guid.NewGuid().ToString();
                }

                // Get context from database based on message type
                var context = await GetRelevantContextAsync(request.Message, request.MessageType);

                // Create enhanced prompt with context
                var enhancedPrompt = CreateEnhancedPrompt(request.Message, context);

                // Get AI response from Gemini
                var aiResponse = await GetGeminiResponseAsync(enhancedPrompt);

                // Create suggestions based on message type
                var suggestions = GenerateSuggestions(request.MessageType, request.Message);

                // Save conversation to database
                await SaveChatMessageAsync(new ChatMessage
                {
                    Message = request.Message,
                    Response = aiResponse,
                    SessionId = request.SessionId,
                    UserId = request.UserId,
                    MessageType = request.MessageType,
                    Context = JsonConvert.SerializeObject(context)
                });

                return new ChatResponse
                {
                    Response = aiResponse,
                    SessionId = request.SessionId,
                    Suggestions = suggestions,
                    Data = context
                };
            }
            catch (Exception ex)
            {
                return new ChatResponse
                {
                    Response = "I apologize, but I'm having trouble processing your request right now. Please try again later.",
                    SessionId = request.SessionId ?? Guid.NewGuid().ToString()
                };
            }
        }

        private async Task<object> GetRelevantContextAsync(string message, ChatMessageType messageType)
        {
            var context = new Dictionary<string, object>();

            switch (messageType)
            {
                case ChatMessageType.HotelSearch:
                    var hotels = await _context.HotelCompanies
                        .Include(h => h.Rooms)
                        .Take(5)
                        .Select(h => new { h.Name, h.Location, h.Rating, h.Description })
                        .ToListAsync();
                    context["hotels"] = hotels;
                    break;

                case ChatMessageType.FlightSearch:
                    var flights = await _context.Flights
                        .Include(f => f.FlightCompany)
                        .Take(5)
                        .Select(f => new {
                            f.DepartureAirport,
                            f.ArrivalAirport,
                            f.DepartureTime,
                            f.EconomyPrice,
                            CompanyName = f.FlightCompany.Name
                        })
                        .ToListAsync();
                    context["flights"] = flights;
                    break;

                case ChatMessageType.CarRental:
                    var cars = await _context.Cars
                        .Include(c => c.RentalCompany)
                        .Take(5)
                        .Select(c => new {
                            c.Model,
                            c.Location,
                            c.Price,
                            c.Capacity,
                            CompanyName = c.RentalCompany.Name
                        })
                        .ToListAsync();
                    context["cars"] = cars;
                    break;

                case ChatMessageType.TourSearch:
                    var tours = await _context.Tours
                        .Include(t => t.TourCompany)
                        .Take(5)
                        .Select(t => new {
                            t.Name,
                            t.Destination,
                            t.Price,
                            t.Category,
                            CompanyName = t.TourCompany.Name
                        })
                        .ToListAsync();
                    context["tours"] = tours;
                    break;

                case ChatMessageType.BookingInquiry:
                    // Get user's recent bookings if UserId is available
                    context["booking_types"] = new[] { "Hotel", "Flight", "Car", "Tour" };
                    break;

                default:
                    context["general_info"] = "I'm here to help you with travel bookings, hotels, flights, car rentals, and tours.";
                    break;
            }

            return context;
        }

        private string CreateEnhancedPrompt(string userMessage, object context)
        {
            var contextJson = JsonConvert.SerializeObject(context, Formatting.Indented);

            return $@"
You are a helpful travel booking assistant. Use the following context to provide accurate and helpful responses.

Context: {contextJson}

User Message: {userMessage}

Guidelines:
- Be friendly and professional
- Provide specific information when available
- If searching for bookings, mention available options from the context
- Always offer to help with bookings
- Keep responses concise but informative
- If you don't have specific information, guide the user on how to get it

Response:";
        }

        private async Task<string> GetGeminiResponseAsync(string prompt)
        {
            try
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_geminiApiUrl}?key={_geminiApiKey}";
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var geminiResponse = JsonConvert.DeserializeObject<GeminiResponse>(responseContent);

                    return geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text
                           ?? "I apologize, but I couldn't generate a response at the moment.";
                }
                else
                {
                    return "I'm having trouble connecting to my AI service. Please try again.";
                }
            }
            catch (Exception)
            {
                return "I encountered an error while processing your request. Please try again.";
            }
        }

        private List<SuggestionButton> GenerateSuggestions(ChatMessageType messageType, string message)
        {
            return messageType switch
            {
                ChatMessageType.General => new List<SuggestionButton>
                {
                    new() { Text = "Find Hotels", Action = "search_hotels" },
                    new() { Text = "Search Flights", Action = "search_flights" },
                    new() { Text = "Rent a Car", Action = "search_cars" },
                    new() { Text = "Browse Tours", Action = "search_tours" }
                },
                ChatMessageType.HotelSearch => new List<SuggestionButton>
                {
                    new() { Text = "Show Available Rooms", Action = "show_rooms" },
                    new() { Text = "Check Prices", Action = "check_prices" },
                    new() { Text = "View Amenities", Action = "view_amenities" }
                },
                ChatMessageType.FlightSearch => new List<SuggestionButton>
                {
                    new() { Text = "Compare Prices", Action = "compare_flights" },
                    new() { Text = "Check Schedules", Action = "check_schedules" },
                    new() { Text = "Seat Selection", Action = "seat_selection" }
                },
                _ => new List<SuggestionButton>()
            };
        }

        public async Task<List<ChatMessage>> GetChatHistoryAsync(string sessionId)
        {
            return await _context.ChatMessages
                .Where(m => m.SessionId == sessionId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task SaveChatMessageAsync(ChatMessage message)
        {
            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
        }


    }
}
