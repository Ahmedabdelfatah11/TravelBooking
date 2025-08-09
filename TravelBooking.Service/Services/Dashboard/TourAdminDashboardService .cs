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

        public async  Task<object> GetStatsForTourCompany(int tourCompanyId)
        {
            var totalTours = (await _tourRepo.GetAllAsync(t => t.TourCompanyId == tourCompanyId)).Count;
            var totalBookings = (await _bookingRepo.GetAllAsync(b => b.Tour.TourCompanyId == tourCompanyId)).Count;
            var activeBookings = (await _bookingRepo.GetAllAsync(b => b.Tour.TourCompanyId == tourCompanyId && b.Status == Status.Confirmed)).Count;
            var totalRevenue = (await _bookingRepo.GetAllAsync(
                b => b.Tour.TourCompanyId == tourCompanyId && b.Status == Status.Confirmed))
                .Sum(b => b.TotalPrice);

            return new
            {
                TotalTours = totalTours,
                TotalBookings = totalBookings,
                ActiveBookings = activeBookings,
                TotalRevenue = totalRevenue
            };
        }
    }
}
