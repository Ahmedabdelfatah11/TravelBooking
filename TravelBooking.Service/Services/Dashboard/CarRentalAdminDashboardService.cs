using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;

namespace TravelBooking.Service.Services.Dashboard
{
    public class CarRentalAdminDashboardService : ICarRentalAdminDashboardService
    {
        private readonly IGenericRepository<Car> _carRepo;
        private readonly IGenericRepository<Booking> _bookingRepo;

        public CarRentalAdminDashboardService(
            IGenericRepository<Car> carRepo,
            IGenericRepository<Booking> bookingRepo)
        {
            _carRepo = carRepo;
            _bookingRepo = bookingRepo;
        }

        public async Task<object> GetStatsForCarRentalCompany(int rentalCompanyId)
        {
            var totalCars = (await _carRepo.GetAllAsync(c => c.RentalCompanyId == rentalCompanyId)).Count;
            var totalBookings = (await _bookingRepo.GetAllAsync(b => b.Car.RentalCompanyId == rentalCompanyId)).Count;
            var activeBookings = (await _bookingRepo.GetAllAsync(
                b => b.Car.RentalCompanyId == rentalCompanyId && b.Status == Status.Confirmed)).Count;

            var totalRevenue = (await _bookingRepo.GetAllAsync(
                b => b.Car.RentalCompanyId == rentalCompanyId && b.Status == Status.Confirmed))
                .Sum(b => b.TotalPrice);

            // Chart Data - آخر 30 يوم
            var last30Days = DateTime.UtcNow.Date.AddDays(-29);
            var bookingsLast30Days = await _bookingRepo.GetAllAsync(
                b => b.Car.RentalCompanyId == rentalCompanyId &&
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
                TotalCars = totalCars,
                TotalBookings = totalBookings,
                ActiveBookings = activeBookings,
                TotalRevenue = totalRevenue,
                BookingsChart = chartData
            };
        }
    }
}
