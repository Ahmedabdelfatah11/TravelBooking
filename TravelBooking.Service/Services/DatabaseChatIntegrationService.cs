using Microsoft.EntityFrameworkCore;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Repository.Data;

namespace TravelBooking.Service.Services
{
    public class DatabaseChatIntegrationService
    {
        private readonly AppDbContext _context;
        private readonly IGenericRepository<Car> _carRepo;
        private readonly IGenericRepository<Room> _roomRepo;
        private readonly IGenericRepository<Flight> _flightRepo;
        private readonly IGenericRepository<Tour> _tourRepo;
        private readonly IGenericRepository<Booking> _bookingRepo;

        public DatabaseChatIntegrationService(
            AppDbContext context,
            IGenericRepository<Car> carRepo,
            IGenericRepository<Room> roomRepo,
            IGenericRepository<Flight> flightRepo,
            IGenericRepository<Tour> tourRepo,
            IGenericRepository<Booking> bookingRepo)
        {
            _context = context;
            _carRepo = carRepo;
            _roomRepo = roomRepo;
            _flightRepo = flightRepo;
            _tourRepo = tourRepo;
            _bookingRepo = bookingRepo;
        }

        public async Task<List<Car>> SearchCarsAsync(CarSearchCriteria criteria)
        {
            var query = _context.Cars.AsQueryable()
                .Include(c => c.RentalCompany)
                .Where(c => c.IsAvailable);

            if (!string.IsNullOrEmpty(criteria.Location))
            {
                query = query.Where(c => c.Location.Contains(criteria.Location));
            }

            if (criteria.MinPrice.HasValue)
            {
                query = query.Where(c => c.Price >= criteria.MinPrice);
            }

            if (criteria.MaxPrice.HasValue)
            {
                query = query.Where(c => c.Price <= criteria.MaxPrice);
            }

            if (criteria.MinCapacity.HasValue)
            {
                query = query.Where(c => c.Capacity >= criteria.MinCapacity);
            }

           
            if (!string.IsNullOrEmpty(criteria.SearchText))
            {
                query = query.Where(c =>
                    c.Model.Contains(criteria.SearchText) ||
                    c.Description.Contains(criteria.SearchText) ||
                    c.RentalCompany.Name.Contains(criteria.SearchText));
            }

            return await query.OrderBy(c => c.Price).Take(10).ToListAsync();
        }

        public async Task<List<Room>> SearchRoomsAsync(RoomSearchCriteria criteria)
        {
            var query = _context.Rooms.AsQueryable()
                .Include(r => r.Hotel)
                .Include(r => r.Images)
                .Where(r => r.IsAvailable);

            if (!string.IsNullOrEmpty(criteria.Location))
            {
                query = query.Where(r => r.Hotel.Location.Contains(criteria.Location));
            }

            if (criteria.MinPrice.HasValue)
            {
                query = query.Where(r => r.Price >= criteria.MinPrice);
            }

            if (criteria.MaxPrice.HasValue)
            {
                query = query.Where(r => r.Price <= criteria.MaxPrice);
            }

            if (criteria.RoomType.HasValue)
            {
                query = query.Where(r => r.RoomType == criteria.RoomType);
            }

            if (criteria.CheckIn.HasValue && criteria.CheckOut.HasValue)
            {
                // Check room availability for the requested dates
                query = query.Where(r =>
                    r.From <= criteria.CheckIn &&
                    r.To >= criteria.CheckOut &&
                    !r.Bookings.Any(b =>
                        b.Status != Status.Cancelled &&
                        b.StartDate < criteria.CheckOut &&
                        criteria.CheckIn < b.EndDate));
            }

            if (!string.IsNullOrEmpty(criteria.SearchText))
            {
                query = query.Where(r =>
                    r.Hotel.Name.Contains(criteria.SearchText) ||
                    r.Description.Contains(criteria.SearchText) ||
                    r.Hotel.Location.Contains(criteria.SearchText));
            }

            return await query.OrderBy(r => r.Price).Take(10).ToListAsync();
        }

        public async Task<List<Flight>> SearchFlightsAsync(FlightSearchCriteria criteria)
        {
            var query = _context.Flights.AsQueryable()
                .Include(f => f.FlightCompany);

            if (!string.IsNullOrEmpty(criteria.DepartureAirport))
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Flight, FlightCompany?>)query.Where(f => f.DepartureAirport.Contains(criteria.DepartureAirport));
            }

            if (!string.IsNullOrEmpty(criteria.ArrivalAirport))
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Flight, FlightCompany?>)query.Where(f => f.ArrivalAirport.Contains(criteria.ArrivalAirport));
            }

            if (criteria.DepartureDate.HasValue)
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Flight, FlightCompany?>)query.Where(f => f.DepartureTime.Date == criteria.DepartureDate.Value.Date);
            }

            if (criteria.MaxPrice.HasValue)
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Flight, FlightCompany?>)query.Where(f =>
                    f.EconomyPrice <= criteria.MaxPrice ||
                    f.BusinessPrice <= criteria.MaxPrice ||
                    f.FirstClassPrice <= criteria.MaxPrice);
            }

            if (criteria.SeatClass.HasValue)
            {
                switch (criteria.SeatClass.Value)
                {
                    case SeatClass.Economy:
                        query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Flight, FlightCompany?>)query.Where(f => f.EconomySeats > 0);
                        break;
                    case SeatClass.Business:
                        query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Flight, FlightCompany?>)query.Where(f => f.BusinessSeats > 0);
                        break;
                    case SeatClass.FirstClass:
                        query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Flight, FlightCompany?>)query.Where(f => f.FirstClassSeats > 0);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(criteria.SearchText))
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Flight, FlightCompany?>)query.Where(f =>
                    f.FlightCompany.Name.Contains(criteria.SearchText) ||
                    f.DepartureAirport.Contains(criteria.SearchText) ||
                    f.ArrivalAirport.Contains(criteria.SearchText));
            }

            return await query.OrderBy(f => f.EconomyPrice).Take(10).ToListAsync();
        }

        public async Task<List<Tour>> SearchToursAsync(TourSearchCriteria criteria)
        {
            var query = _context.Tours.AsQueryable()
                .Include(t => t.TourCompany)
                .Where(t => t.TourCompanyId !=null);

            if (!string.IsNullOrEmpty(criteria.Destination))
            {
                query = query.Where(t =>
                    t.Name.Contains(criteria.Destination) ||
                    t.Description.Contains(criteria.Destination));
            }

            if (criteria.MinPrice.HasValue)
            {
                query = query.Where(t => t.Price >= criteria.MinPrice);
            }

            if (criteria.MaxPrice.HasValue)
            {
                query = query.Where(t => t.Price <= criteria.MaxPrice);
            }

            if (criteria.StartDate.HasValue)
            {
                query = query.Where(t => t.StartDate >= criteria.StartDate);
            }

            if (criteria.Duration.HasValue)
            {
                query = query.Where(t =>
                    EF.Functions.DateDiffDay(t.StartDate, t.EndDate) <= criteria.Duration);
            }

            if (!string.IsNullOrEmpty(criteria.SearchText))
            {
                query = query.Where(t =>
                    t.Name.Contains(criteria.SearchText) ||
                    t.Description.Contains(criteria.SearchText) ||
                    t.TourCompany.Name.Contains(criteria.SearchText));
            }

            return await query.OrderBy(t => t.StartDate).Take(10).ToListAsync();
        }

        public async Task<UserBookingSummary> GetUserBookingSummaryAsync(string userId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Car).ThenInclude(c => c.RentalCompany)
                .Include(b => b.Room).ThenInclude(r => r.Hotel)
                .Include(b => b.Flight).ThenInclude(f => f.FlightCompany)
                .Include(b => b.Tour).ThenInclude(t => t.TourCompany)
                .OrderByDescending(b => b.Id)
                .ToListAsync();

            return new UserBookingSummary
            {
                TotalBookings = bookings.Count,
                ActiveBookings = bookings.Count(b => b.Status == Status.Confirmed),
                PendingBookings = bookings.Count(b => b.Status == Status.Pending),
                CancelledBookings = bookings.Count(b => b.Status == Status.Cancelled),
                RecentBookings = bookings.Take(5).Select(b => new BookingSummary
                {
                    Id = b.Id,
                    Type = b.BookingType.ToString(),
                    ServiceName = GetServiceName(b),
                    Status = b.Status.ToString(),
                    StartDate = b.StartDate,
                    EndDate = b.EndDate
                }).ToList(),
                TotalSpent = bookings.Where(b => b.Status == Status.Confirmed)
                    .Sum(b => GetBookingPrice(b))
            };
        }

        public async Task<List<RecommendedService>> GetPersonalizedRecommendationsAsync(string userId)
        {
            var userBookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Car).ThenInclude(c => c.RentalCompany)
                .Include(b => b.Room).ThenInclude(r => r.Hotel)
                .Include(b => b.Flight).ThenInclude(f => f.FlightCompany)
                .Include(b => b.Tour).ThenInclude(t => t.TourCompany)
                .ToListAsync();

            var recommendations = new List<RecommendedService>();

            // Analyze user preferences based on booking history
            var preferredPriceRange = CalculatePreferredPriceRange(userBookings);
            var preferredLocations = GetPreferredLocations(userBookings);
            var preferredServiceTypes = GetPreferredServiceTypes(userBookings);

            // Get recommendations based on preferences
            if (preferredServiceTypes.Contains("Car"))
            {
                var cars = await SearchCarsAsync(new CarSearchCriteria
                {
                    MinPrice = preferredPriceRange.Min * 0.8m,
                    MaxPrice = preferredPriceRange.Max * 1.2m,
                    SearchText = preferredLocations.FirstOrDefault()
                });

                recommendations.AddRange(cars.Take(3).Select(c => new RecommendedService
                {
                    Id = c.Id,
                    Type = "Car",
                    Name = c.Model,
                    Price = c.Price ?? 0,
                    Location = c.Location,
                    Description = c.Description,
                    Reason = "بناءً على حجوزاتك السابقة للسيارات"
                }));
            }

            if (preferredServiceTypes.Contains("Room"))
            {
                var rooms = await SearchRoomsAsync(new RoomSearchCriteria
                {
                    MinPrice = preferredPriceRange.Min * 0.8m,
                    MaxPrice = preferredPriceRange.Max * 1.2m,
                    SearchText = preferredLocations.FirstOrDefault()
                });

                recommendations.AddRange(rooms.Take(3).Select(r => new RecommendedService
                {
                    Id = r.Id,
                    Type = "Room",
                    Name = $"{r.RoomType} Room at {r.Hotel?.Name}",
                    Price = r.Price,
                    Location = r.Hotel?.Location,
                    Description = r.Description,
                    Reason = "بناءً على حجوزاتك السابقة للفنادق"
                }));
            }

            return recommendations.Take(5).ToList();
        }

        public async Task<TravelInsights> GetTravelInsightsAsync(string userId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId && b.Status == Status.Confirmed)
                .Include(b => b.Car).ThenInclude(c => c.RentalCompany)
                .Include(b => b.Room).ThenInclude(r => r.Hotel)
                .Include(b => b.Flight).ThenInclude(f => f.FlightCompany)
                .Include(b => b.Tour).ThenInclude(t => t.TourCompany)
                .ToListAsync();

            if (!bookings.Any())
            {
                return new TravelInsights
                {
                    Message = "لا توجد بيانات كافية لتحليل عادات السفر. ابدأ بحجز أول رحلة!"
                };
            }

            var totalSpent = bookings.Sum(b => GetBookingPrice(b));
            var averageSpent = totalSpent / bookings.Count;
            var mostVisitedDestinations = GetMostVisitedDestinations(bookings);
            var preferredServiceType = GetMostUsedServiceType(bookings);
            var travelFrequency = CalculateTravelFrequency(bookings);

            return new TravelInsights
            {
                TotalTrips = bookings.Count,
                TotalSpent = totalSpent,
                AverageSpentPerTrip = averageSpent,
                MostVisitedDestination = mostVisitedDestinations.FirstOrDefault(),
                PreferredServiceType = preferredServiceType,
                TravelFrequency = travelFrequency,
                Recommendations = new List<string>
                {
                    GenerateBudgetRecommendation(averageSpent),
                    GenerateDestinationRecommendation(mostVisitedDestinations),
                    GenerateServiceRecommendation(preferredServiceType)
                }
            };
        }

        private string GetServiceName(Booking booking)
        {
            return booking.BookingType switch
            {
                BookingType.Car => booking.Car?.Model ?? "Car Booking",
                BookingType.Room => $"{booking.Room?.RoomType} at {booking.Room?.Hotel?.Name}" ?? "Room Booking",
                BookingType.Flight => $"{booking.Flight?.DepartureAirport} → {booking.Flight?.ArrivalAirport}" ?? "Flight Booking",
                BookingType.Tour => booking.Tour?.Name ?? "Tour Booking",
                _ => "Unknown Booking"
            };
        }

        private decimal GetBookingPrice(Booking booking)
        {
            return booking.BookingType switch
            {
                BookingType.Car => booking.Car?.Price ?? 0,
                BookingType.Room => booking.Room?.Price ?? 0,
                BookingType.Flight => booking.Flight?.GetPrice(booking.SeatClass) ?? 0,
                BookingType.Tour => booking.Tour?.Price ?? 0,
                _ => 0
            };
        }

        private (decimal Min, decimal Max) CalculatePreferredPriceRange(List<Booking> bookings)
        {
            if (!bookings.Any()) return (0, 1000);

            var prices = bookings.Select(GetBookingPrice).Where(p => p > 0).ToList();
            if (!prices.Any()) return (0, 1000);

            return (prices.Min(), prices.Max());
        }

        private List<string> GetPreferredLocations(List<Booking> bookings)
        {
            var locations = new List<string>();

            foreach (var booking in bookings)
            {
                switch (booking.BookingType)
                {
                    case BookingType.Car:
                        if (!string.IsNullOrEmpty(booking.Car?.Location))
                            locations.Add(booking.Car.Location);
                        break;
                    case BookingType.Room:
                        if (!string.IsNullOrEmpty(booking.Room?.Hotel?.Location))
                            locations.Add(booking.Room.Hotel.Location);
                        break;
                    case BookingType.Flight:
                        if (!string.IsNullOrEmpty(booking.Flight?.ArrivalAirport))
                            locations.Add(booking.Flight.ArrivalAirport);
                        break;
                }
            }

            return locations.GroupBy(l => l)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(3)
                .ToList();
        }

        private List<string> GetPreferredServiceTypes(List<Booking> bookings)
        {
            return bookings.GroupBy(b => b.BookingType.ToString())
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .ToList();
        }

        private List<string> GetMostVisitedDestinations(List<Booking> bookings)
        {
            return GetPreferredLocations(bookings);
        }

        private string GetMostUsedServiceType(List<Booking> bookings)
        {
            return bookings.GroupBy(b => b.BookingType)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key.ToString() ?? "Unknown";
        }

        private string CalculateTravelFrequency(List<Booking> bookings)
        {
            if (bookings.Count < 2) return "مسافر جديد";

            var oldestBooking = bookings.Min(b => b.StartDate);
            var monthsSinceFirst = (DateTime.UtcNow - oldestBooking).Days / 30.0;
            var frequency = bookings.Count / monthsSinceFirst;

            return frequency switch
            {
                > 1 => "مسافر متكرر",
                > 0.5 => "مسافر منتظم",
                > 0.25 => "مسافر عادي",
                _ => "مسافر متقطع"
            };
        }

        private string GenerateBudgetRecommendation(decimal averageSpent)
        {
            if (averageSpent > 5000)
                return "يمكنك الاستفادة من عروضنا المميزة للرحلات الفاخرة";
            else if (averageSpent > 2000)
                return "ننصحك بالحجز المبكر للحصول على خصومات أفضل";
            else
                return "تحقق من عروضنا الاقتصادية المميزة";
        }

        private string GenerateDestinationRecommendation(List<string> destinations)
        {
            if (destinations.Contains("القاهرة"))
                return "جرب زيارة الأقصر لاستكمال رحلتك التاريخية";
            else if (destinations.Contains("الغردقة"))
                return "ننصحك بزيارة شرم الشيخ للاستمتاع بتجربة غوص مختلفة";
            else
                return "استكشف وجهات جديدة في مصر";
        }

        private string GenerateServiceRecommendation(string preferredType)
        {
            return preferredType switch
            {
                "Car" => "جرب حجز باقة متكاملة مع الفندق لتوفير أكبر",
                "Room" => "أضف رحلة سياحية مع حجز الفندق",
                "Flight" => "احجز السيارة مسبقاً لضمان الراحة عند الوصول",
                _ => "جرب خدماتنا المتنوعة لتجربة سفر أفضل"
            };
        }
    }

    // Search criteria classes
    public class CarSearchCriteria
    {
        public string? Location { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinCapacity { get; set; }
        public DateTime? PickupDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? SearchText { get; set; }
    }

    public class RoomSearchCriteria
    {
        public string? Location { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public RoomType? RoomType { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string? SearchText { get; set; }
    }

    public class FlightSearchCriteria
    {
        public string? DepartureAirport { get; set; }
        public string? ArrivalAirport { get; set; }
        public DateTime? DepartureDate { get; set; }
        public decimal? MaxPrice { get; set; }
        public SeatClass? SeatClass { get; set; }
        public string? SearchText { get; set; }
    }

    public class TourSearchCriteria
    {
        public string? Destination { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public int? Duration { get; set; }
        public string? SearchText { get; set; }
    }

    // Response classes
    public class UserBookingSummary
    {
        public int TotalBookings { get; set; }
        public int ActiveBookings { get; set; }
        public int PendingBookings { get; set; }
        public int CancelledBookings { get; set; }
        public List<BookingSummary> RecentBookings { get; set; } = new();
        public decimal TotalSpent { get; set; }
    }

    public class BookingSummary
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string ServiceName { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class RecommendedService
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string Reason { get; set; }
    }

    public class TravelInsights
    {
        public string? Message { get; set; }
        public int TotalTrips { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AverageSpentPerTrip { get; set; }
        public string? MostVisitedDestination { get; set; }
        public string? PreferredServiceType { get; set; }
        public string? TravelFrequency { get; set; }
        public List<string> Recommendations { get; set; } = new();
    }
}