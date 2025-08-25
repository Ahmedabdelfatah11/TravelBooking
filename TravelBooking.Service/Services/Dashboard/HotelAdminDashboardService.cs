using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;

namespace TravelBooking.Service.Services.Dashboard
{
    public  class HotelAdminDashboardService : IHotelAdminDashboardService
    {

        private readonly IGenericRepository<Room> _roomRepo;
        private readonly IGenericRepository<Booking> _bookingRepo;

        public HotelAdminDashboardService(
            IGenericRepository<Room> roomRepo,
            IGenericRepository<Booking> bookingRepo)
        {
            _roomRepo = roomRepo;
            _bookingRepo = bookingRepo;
        }
        public async Task<object> GetStatsForHotel(int hotelId)
        {
            var totalRooms = await _roomRepo.GetCountAsync(r => r.HotelId == hotelId);
            var totalBookings = await _bookingRepo.GetCountAsync(b => b.Room.HotelId == hotelId);
            var activeBookings = await _bookingRepo.GetCountAsync(b => b.Room.HotelId == hotelId && b.Status == Status.Confirmed);
            var totalRevenue = (await _bookingRepo.GetAllAsync(b => b.Room.HotelId == hotelId && b.Status == Status.Confirmed))
                                .Sum(b => b.TotalPrice);
            // Chart Data: 
            var last30Days = DateTime.UtcNow.Date.AddDays(-29);
            var bookingsLast30Days = await _bookingRepo.GetAllAsync(
                b => b.Room.HotelId == hotelId &&
                     b.Status == Status.Confirmed &&
                     b.StartDate >= last30Days);

            var chartData = bookingsLast30Days
                .GroupBy(b => b.StartDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToList();

            return new
            {
                TotalRooms = totalRooms,
                TotalBookings = totalBookings,
                ActiveBookings = activeBookings,
                TotalRevenue = totalRevenue,
                BookingChart = chartData
            };
        }
    }
}
