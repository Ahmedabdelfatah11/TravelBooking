using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;

namespace TravelBooking.Service.Services.Dashboard
{
    public class FlightAdminDashboardService : IFlightAdminDashboardService
    {
        private readonly IGenericRepository<Flight> _flightRepo;
        private readonly IGenericRepository<Booking> _bookingRepo;

        public FlightAdminDashboardService(
            IGenericRepository<Flight> flightRepo,
            IGenericRepository<Booking> bookingRepo)
        {
            _flightRepo = flightRepo;
            _bookingRepo = bookingRepo;
        }
        public async Task<object> GetStatsForFlightCompany(int flightCompanyId)
        {
            var totalFlights = (await _flightRepo.GetAllAsync(f => f.FlightCompanyId == flightCompanyId)).Count;
            var totalBookings = (await _bookingRepo.GetAllAsync(b => b.Flight.FlightCompanyId == flightCompanyId)).Count;
            var activeBookings = (await _bookingRepo.GetAllAsync(b => b.Flight.FlightCompanyId == flightCompanyId && b.Status == Status.Confirmed)).Count;
            var totalRevenue = (await _bookingRepo.GetAllAsync(
                b => b.Flight.FlightCompanyId == flightCompanyId && b.Status == Status.Confirmed))
                .Sum(b => b.TotalPrice);

            // Chart Data: 
            var last30Days = DateTime.UtcNow.Date.AddDays(-29);
            var bookingsLast30Days = await _bookingRepo.GetAllAsync(
                b => b.Flight.FlightCompanyId == flightCompanyId &&
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
                TotalFlights = totalFlights,
                TotalBookings = totalBookings,
                ActiveBookings = activeBookings,
                TotalRevenue = totalRevenue,
                BookingsChart = chartData
            };
        }
    }
}
