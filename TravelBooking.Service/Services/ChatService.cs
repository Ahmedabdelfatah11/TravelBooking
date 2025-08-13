using System.Text;
using System.Text.RegularExpressions;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications;
using TravelBooking.Core.Specifications.CarSpecs;
using TravelBooking.Core.Specifications.FlightSpecs;
using TravelBooking.Core.Specifications.RoomSpecs;
using TravelBooking.Core.Specifications.TourSpecs;

namespace TravelBooking.Service.Services
{
    public class ChatService
    {
        private readonly GeminiService _geminiService;
        private readonly ChatHistoryService _historyService;
        private readonly IGenericRepository<Car> _carRepo;
        private readonly IGenericRepository<Room> _roomRepo;
        private readonly IGenericRepository<Flight> _flightRepo;
        private readonly IGenericRepository<Tour> _tourRepo;
        private readonly IGenericRepository<Booking> _bookingRepo;

        public ChatService(
            GeminiService gemini,
            ChatHistoryService history,
            IGenericRepository<Car> carRepo,
            IGenericRepository<Room> roomRepo,
            IGenericRepository<Flight> flightRepo,
            IGenericRepository<Tour> tourRepo,
            IGenericRepository<Booking> bookingRepo)
        {
            _geminiService = gemini;
            _historyService = history;
            _carRepo = carRepo;
            _roomRepo = roomRepo;
            _flightRepo = flightRepo;
            _tourRepo = tourRepo;
            _bookingRepo = bookingRepo;
        }

        public async Task<ChatResponse> GetResponseAsync(string userInput, string userId)
        {
            var chatHistory = _historyService.GetLatestMessages(userId, 3);
            var historyContext = BuildHistoryContext(chatHistory);

            // تحديد نوع الاستفسار
            var queryType = DetermineQueryType(userInput);

            // جلب البيانات المناسبة من قاعدة البيانات
            var searchResults = await SearchRelevantServices(userInput, queryType);

            // بناء السياق
            var contextText = BuildEnhancedContext(searchResults);

            // إنشاء prompt محسن
            string prompt = BuildEnhancedPrompt(userInput, historyContext, contextText, queryType);

            var response = await _geminiService.GenerateAnswerAsync(prompt);
            var processedResponse = PostProcess(response);

            await _historyService.SaveMessageAsync(userInput, processedResponse, userId);

            return new ChatResponse
            {
                Message = processedResponse,
                QueryType = queryType,
                SuggestedActions = GenerateSuggestedActions(queryType, searchResults),
                RecommendedServices = searchResults
            };
        }

        private QueryType DetermineQueryType(string input)
        {
            input = input.ToLower();

            if (input.Contains("car") || input.Contains("سيارة") || input.Contains("rent"))
                return QueryType.Car;

            if (input.Contains("hotel") || input.Contains("room") || input.Contains("فندق") || input.Contains("غرف"))
                return QueryType.Hotel;

            if (input.Contains("flight") || input.Contains("طيران") || input.Contains("plane"))
                return QueryType.Flight;

            if (input.Contains("tour") || input.Contains("جولة") || input.Contains("رحلة"))
                return QueryType.Tour;

            if (input.Contains("book") || input.Contains("حجز") || input.Contains("reservation"))
                return QueryType.Booking;

            return QueryType.General;
        }

        private async Task<ServiceSearchResults> SearchRelevantServices(string query, QueryType queryType)
        {
            var results = new ServiceSearchResults();

            switch (queryType)
            {
                case QueryType.Car:
                    var carSpec = new CarSpecifications(new CarSpecParams
                    {
                        Search = ExtractSearchTerms(query),
                        PageSize = 5
                    });
                    results.Cars = await _carRepo.GetAllWithSpecAsync(carSpec);
                    break;

                case QueryType.Hotel:
                    var roomSpec = new RoomSpecification(new RoomSpecParams
                    {
                        Search = ExtractSearchTerms(query),
                        PageSize = 5
                    });
                    results.Rooms = await _roomRepo.GetAllWithSpecAsync(roomSpec);
                    break;

                case QueryType.Flight:
                    var flightSpec = new FlightSpecs(new FlightSpecParams
                    {
                        Search = ExtractSearchTerms(query),
                        PageSize = 5
                    });
                    results.Flights = await _flightRepo.GetAllWithSpecAsync(flightSpec);
                    break;

                case QueryType.Tour:
                    var tourSpec = new ToursSpecification(new TourSpecParams
                    {
                        Search = ExtractSearchTerms(query),
                        PageSize = 5
                    }
                        );
                    results.Tours = await _tourRepo.GetAllWithSpecAsync(tourSpec);
                    break;

                default:
                    // General search - get a mix of all services
                    results = await GetMixedResults(query);
                    break;
            }

            return results;
        }

        private async Task<ServiceSearchResults> GetMixedResults(string query)
        {
            var results = new ServiceSearchResults();

            // Get top 2-3 from each category
            var carSpec = new CarSpecifications(new CarSpecParams { Search = query, PageSize = 3 });
            var roomSpec = new RoomSpecification(new RoomSpecParams { Search = query, PageSize = 3 });
            var flightSpec = new FlightSpecs(new FlightSpecParams { Search = query, PageSize = 3 });

            results.Cars = await _carRepo.GetAllWithSpecAsync(carSpec);
            results.Rooms = await _roomRepo.GetAllWithSpecAsync(roomSpec);
            Console.WriteLine(results.Rooms);
            results.Flights = await _flightRepo.GetAllWithSpecAsync(flightSpec);

            return results;
        }

        private string BuildEnhancedPrompt(string userInput, string historyContext, string contextText, QueryType queryType)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("أنت مساعد سفر مصري محترف ومتخصص في السياحة المصرية.");
            prompt.AppendLine();

            // إضافة السياق التاريخي
            if (!string.IsNullOrEmpty(historyContext))
            {
                prompt.AppendLine("المحادثة السابقة:");
                prompt.AppendLine(historyContext);
                prompt.AppendLine();
            }

            prompt.AppendLine($"استفسار المستخدم: \"{userInput}\"");
            prompt.AppendLine();

            // إضافة الخدمات المتاحة
            if (!string.IsNullOrEmpty(contextText))
            {
                prompt.AppendLine("الخدمات المتاحة:");
                prompt.AppendLine(contextText);
                prompt.AppendLine();
            }

            // إرشادات خاصة بنوع الاستفسار
            prompt.AppendLine("إرشادات الرد:");
            prompt.AppendLine("1. كن ودودًا ومرحبًا");
            prompt.AppendLine("2. اقترح باقات سفر متكاملة عند الحاجة");
            prompt.AppendLine("3. اذكر المعالم المصرية الثقافية المناسبة");
            prompt.AppendLine("4. أضف نصائح سفر عملية");
            prompt.AppendLine("5. اطرح أسئلة متابعة لتقديم مساعدة أفضل");
            prompt.AppendLine("6. قدم خيارات حجز مع \"احجز الآن\"");
            prompt.AppendLine("7. اذكر التجارب المصرية الخاصة (رحلات النيل، سفاري الصحراء، إلخ)");

            switch (queryType)
            {
                case QueryType.Car:
                    prompt.AppendLine("8. ركز على تفاصيل السيارات والأسعار والمواقع");
                    break;
                case QueryType.Hotel:
                    prompt.AppendLine("8. ركز على تفاصيل الفنادق والغرف والمرافق");
                    break;
                case QueryType.Flight:
                    prompt.AppendLine("8. ركز على تفاصيل الرحلات والأوقات والأسعار");
                    break;
                case QueryType.Booking:
                    prompt.AppendLine("8. ساعد في عملية الحجز وقدم الخطوات اللازمة");
                    break;
            }

            prompt.AppendLine();
            prompt.AppendLine("اختتم دائمًا بسؤال مفيد أو دعوة للحجز.");

            return prompt.ToString();
        }

        private string BuildEnhancedContext(ServiceSearchResults results)
        {
            var sb = new StringBuilder();

            // السيارات
            if (results.Cars?.Any() == true)
            {
                sb.AppendLine("🚗 **السيارات المتاحة:**");
                foreach (var car in results.Cars.Take(5))
                {
                    sb.AppendLine($"• **{car.Model}** - شركة {car.RentalCompany?.Name}");
                    sb.AppendLine($"  📍 الموقع: {car.Location}");
                    sb.AppendLine($"  💰 السعر: {car.Price:N0} جنيه يوميًا");
                    sb.AppendLine($"  👥 السعة: {car.Capacity} راكب");
                    sb.AppendLine($"  ⏰ متاح من: {car.DepartureTime:HH:mm} إلى {car.ArrivalTime:HH:mm}");
                    if (!string.IsNullOrEmpty(car.Description))
                        sb.AppendLine($"  ℹ️ {car.Description}");
                    sb.AppendLine($"  🔗 [احجز الآن] /api/Car/{car.Id}/book");
                    sb.AppendLine();
                }
            }

            // الغرف/الفنادق
            if (results.Rooms?.Any() == true)
            {
                sb.AppendLine("🏨 **الغرف المتاحة:**");
                foreach (var room in results.Rooms.Take(5))
                {
                    sb.AppendLine($"• **غرفة {room.RoomType}** في فندق {room.Hotel?.Name}");
                    sb.AppendLine($"  📍 الموقع: {room.Hotel?.Location}");
                    sb.AppendLine($"  💰 السعر: {room.Price:N0} جنيه ليليًا");
                    sb.AppendLine($"  📅 متاح من: {room.From:yyyy-MM-dd} إلى {room.To:yyyy-MM-dd}");
                    if (!string.IsNullOrEmpty(room.Description))
                        sb.AppendLine($"  ℹ️ {room.Description}");
                    sb.AppendLine($"  🔗 [احجز الآن] /api/Room/{room.Id}/book");
                    sb.AppendLine();
                }
            }

            // الرحلات الجوية
            if (results.Flights?.Any() == true)
            {
                sb.AppendLine("✈️ **الرحلات الجوية المتاحة:**");
                foreach (var flight in results.Flights.Take(5))
                {
                    sb.AppendLine($"• **{flight.FlightCompany?.Name}**");
                    sb.AppendLine($"  🛫 الإقلاع: {flight.DepartureAirport} في {flight.DepartureTime:yyyy-MM-dd HH:mm}");
                    sb.AppendLine($"  🛬 الهبوط: {flight.ArrivalAirport} في {flight.ArrivalTime:yyyy-MM-dd HH:mm}");
                    sb.AppendLine($"  💰 الدرجة الاقتصادية: {flight.EconomyPrice:N0} جنيه");
                    sb.AppendLine($"  🪑 المقاعد المتاحة: {flight.EconomySeats} اقتصادية، {flight.BusinessSeats} رجال أعمال");
                    sb.AppendLine($"  🔗 [احجز الآن] /api/Flight/{flight.Id}/book");
                    sb.AppendLine();
                }
            }

            // الجولات السياحية
            if (results.Tours?.Any() == true)
            {
                sb.AppendLine("🎯 **الجولات السياحية المتاحة:**");
                foreach (var tour in results.Tours.Take(5))
                {
                    sb.AppendLine($"• **{tour.Name}** - شركة {tour.TourCompany?.Name}");
                    sb.AppendLine($"  📅 من: {tour.StartDate:yyyy-MM-dd} إلى {tour.EndDate:yyyy-MM-dd}");
                    sb.AppendLine($"  💰 السعر: {tour.Price:N0} جنيه");
                    if (!string.IsNullOrEmpty(tour.Description))
                        sb.AppendLine($"  ℹ️ {tour.Description}");
                    sb.AppendLine($"  🔗 [احجز الآن] /api/Tour/{tour.Id}/book");
                    sb.AppendLine();
                }
            }

            return sb.Length == 0 ? "لا توجد خدمات متاحة حاليًا." : sb.ToString();
        }

        private List<string> GenerateSuggestedActions(QueryType queryType, ServiceSearchResults results)
        {
            var actions = new List<string>();

            switch (queryType)
            {
                case QueryType.Car:
                    actions.Add("عرض السيارات المتاحة");
                    actions.Add("مقارنة الأسعار");
                    actions.Add("تصفية حسب السعة");
                    break;
                case QueryType.Hotel:
                    actions.Add("عرض الفنادق المتاحة");
                    actions.Add("تصفية حسب السعر");
                    actions.Add("عرض المرافق");
                    break;
                case QueryType.Flight:
                    actions.Add("البحث عن رحلات");
                    actions.Add("مقارنة الأسعار");
                    actions.Add("اختيار درجة السفر");
                    break;
                case QueryType.General:
                    actions.Add("عرض العروض المميزة");
                    actions.Add("باقات السفر");
                    actions.Add("نصائح السفر");
                    break;
            }

            actions.Add("التحدث مع مستشار");
            return actions;
        }

        private string ExtractSearchTerms(string query)
        {
            // استخراج المصطلحات المهمة من الاستفسار
            var stopWords = new[] { "أريد", "أحتاج", "ابحث", "عن", "في", "من", "إلى", "want", "need", "search", "for", "in", "to", "from" };
            var words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", words.Where(w => !stopWords.Contains(w.ToLower())));
        }

        private string BuildHistoryContext(List<ChatMessage> history)
        {
            if (!history.Any()) return string.Empty;

            var sb = new StringBuilder();
            foreach (var message in history.OrderBy(h => h.CreatedAt))
            {
                sb.AppendLine($"المستخدم: {message.UserInput}");
                sb.AppendLine($"المساعد: {message.GeminiResponse}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private string PostProcess(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "عذرًا، لم أتمكن من العثور على خيارات سفر مناسبة. هل يمكنك تقديم المزيد من التفاصيل؟";

            text = text.Trim();
            text = Regex.Replace(text, @"\*\*(.*?)\*\*", "$1");
            text = Regex.Replace(text, @"\s{2,}", " ");
            //text = Regex.Replace(text, @"\n{3,}", " ");

            if (text.Length > 2000)
                text = text.Substring(0, 2000) + "... [تم اختصار الرد]";

            return text;
        }
    }

    // Supporting classes
    public class ChatResponse
    {
        public string Message { get; set; }
        public QueryType QueryType { get; set; }
        public List<string> SuggestedActions { get; set; } = new();
        public ServiceSearchResults RecommendedServices { get; set; }
    }

    public class ServiceSearchResults
    {
        public IReadOnlyList<Car> Cars { get; set; } = new List<Car>();
        public IReadOnlyList<Room> Rooms { get; set; } = new List<Room>();
        public IReadOnlyList<Flight> Flights { get; set; } = new List<Flight>();
        public IReadOnlyList<Tour> Tours { get; set; } = new List<Tour>();
    }

    public enum QueryType
    {
        General,
        Car,
        Hotel,
        Flight,
        Tour,
        Booking
    }
}