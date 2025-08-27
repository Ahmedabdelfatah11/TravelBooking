using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;

namespace TravelBooking.Service.Services.Dashboard
{
    public class TourAdminDashboardService : ITourAdminDashboardService
    {
        private readonly IGenericRepository<Tour> _tourRepo;
        private readonly IGenericRepository<Booking> _bookingRepo;

        public TourAdminDashboardService(
            IGenericRepository<Tour> tourRepo,
            IGenericRepository<Booking> bookingRepo)
        {
            _tourRepo = tourRepo;
            _bookingRepo = bookingRepo;
        }

        public async Task<object> GetStatsForTourCompany(int tourCompanyId)
        {
            var tourIds = await _tourRepo.GetAllAsync(t => t.TourCompanyId == tourCompanyId);
            var tourIdList = tourIds.Select(t => t.Id).ToList();

            if (!tourIdList.Any())
                return new
                {
                    TotalTours = 0,
                    TotalBookings = 0,
                    ActiveBookings = 0,
                    TotalRevenue = 0m,
                    bookingsChart = new object[0],
                    RecentBookings = new object[0]
                };

            var bookings = await _bookingRepo.GetQueryable()
                .Include(b => b.BookingTickets)
                    .ThenInclude(bt => bt.Ticket)
                .Include(b => b.Tour)
                .Where(b => b.BookingType == BookingType.Tour &&
                            b.TourId.HasValue &&
                            tourIdList.Contains(b.TourId.Value))
                .Select(b => new
                {
                    b.Id,
                    b.Status,
                    b.StartDate,
                    b.EndDate,
                    b.User,
                    b.Tour,
                    TotalPrice = b.BookingTickets.Sum(bt => bt.Ticket.Price * bt.Quantity)
                })
                .ToListAsync();

            var confirmedBookings = bookings.Where(b => b.Status == Status.Confirmed).ToList();

            var totalBookings = bookings.Count;
            var activeBookings = confirmedBookings.Count;
            var totalRevenue = confirmedBookings.Sum(b => b.TotalPrice);

            var recentBookings = confirmedBookings
                .OrderByDescending(b => b.StartDate)
                .Take(5)
                .Select(b => new
                {
                    id = b.Id,
                    customerName = b.User?.UserName ?? "Guest",
                    tourName = b.Tour?.Name ?? "Unknown Tour",
                    bookingDate = b.StartDate.ToString("O"),
                    totalAmount = b.TotalPrice,
                    status = b.Status.ToString()
                })
                .ToList();

            var startDate = DateTime.UtcNow.AddDays(-30);
            var bookingsChart = confirmedBookings
                .Where(b => b.StartDate >= startDate)
                .GroupBy(b => b.StartDate.Date)
                .Select(g => new
                {
                    date = g.Key.ToString("yyyy-MM-dd"),
                    count = g.Sum(b => b.TotalPrice) 
                })
                .OrderBy(x => x.date)
                .ToList();

            return new
            {
                TotalTours = tourIdList.Count,
                TotalBookings = totalBookings,
                ActiveBookings = activeBookings,
                TotalRevenue = totalRevenue,
                bookingsChart = bookingsChart,    
                RecentBookings = recentBookings
            };
        }
    }
}