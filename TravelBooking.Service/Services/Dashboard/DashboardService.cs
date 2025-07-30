using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Repository.Data;

namespace TravelBooking.Service.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        // Super Admin Dashboard
        public async Task<object> GetSuperAdminDashboardAsync()
        {
            try
            {
                // User Statistics
                var totalUsers = await _context.Users.CountAsync();
                var totalSuperAdmins = await _context.UserRoles
                    .Join(_context.Roles.Where(r => r.Name == "SuperAdmin"),
                          ur => ur.RoleId, r => r.Id, (ur, r) => ur)
                    .CountAsync();
                var totalHotelAdmins = await _context.UserRoles
                    .Join(_context.Roles.Where(r => r.Name == "HotelAdmin"),
                          ur => ur.RoleId, r => r.Id, (ur, r) => ur)
                    .CountAsync();
                var totalFlightAdmins = await _context.UserRoles
                    .Join(_context.Roles.Where(r => r.Name == "FlightAdmin"),
                          ur => ur.RoleId, r => r.Id, (ur, r) => ur)
                    .CountAsync();
                var totalCarAdmins = await _context.UserRoles
                    .Join(_context.Roles.Where(r => r.Name == "CarRentalAdmin"),
                          ur => ur.RoleId, r => r.Id, (ur, r) => ur)
                    .CountAsync();
                var totalTourAdmins = await _context.UserRoles
                    .Join(_context.Roles.Where(r => r.Name == "TourAdmin"),
                          ur => ur.RoleId, r => r.Id, (ur, r) => ur)
                    .CountAsync();
                var totalRegularUsers = await _context.UserRoles
                    .Join(_context.Roles.Where(r => r.Name == "User"),
                          ur => ur.RoleId, r => r.Id, (ur, r) => ur)
                    .CountAsync();

                // Company Statistics
                var totalHotelCompanies = await _context.HotelCompanies.CountAsync();
                var totalFlightCompanies = await _context.FlightCompanies.CountAsync();
                var totalCarRentalCompanies = await _context.CarRentalCompanies.CountAsync();
                var totalTourCompanies = await _context.TourCompanies.CountAsync();

                // Service Statistics
                var totalRooms = await _context.Rooms.CountAsync();
                var totalFlights = await _context.Flights.CountAsync();
                var totalCars = await _context.Cars.CountAsync();
                var totalTours = await _context.Tours.CountAsync();

                // Booking Statistics
                var totalBookings = await _context.Bookings.CountAsync();
                var pendingBookings = await _context.Bookings.Where(b => b.Status == Status.Pending).CountAsync();
                var confirmedBookings = await _context.Bookings.Where(b => b.Status == Status.Confirmed).CountAsync();
                var cancelledBookings = await _context.Bookings.Where(b => b.Status == Status.Cancelled).CountAsync();

                // Booking by Type
                var hotelBookings = await _context.Bookings.Where(b => b.BookingType == BookingType.Room).CountAsync();
                var flightBookings = await _context.Bookings.Where(b => b.BookingType == BookingType.Flight).CountAsync();
                var carBookings = await _context.Bookings.Where(b => b.BookingType == BookingType.Car).CountAsync();
                var tourBookings = await _context.Bookings.Where(b => b.BookingType == BookingType.Tour).CountAsync();

                var dashboardData = new
                {
                    UserStatistics = new
                    {
                        TotalUsers = totalUsers,
                        SuperAdmins = totalSuperAdmins,
                        HotelAdmins = totalHotelAdmins,
                        FlightAdmins = totalFlightAdmins,
                        CarAdmins = totalCarAdmins,
                        TourAdmins = totalTourAdmins,
                        RegularUsers = totalRegularUsers
                    },
                    CompanyStatistics = new
                    {
                        TotalCompanies = totalHotelCompanies + totalFlightCompanies + totalCarRentalCompanies + totalTourCompanies,
                        HotelCompanies = totalHotelCompanies,
                        FlightCompanies = totalFlightCompanies,
                        CarRentalCompanies = totalCarRentalCompanies,
                        TourCompanies = totalTourCompanies
                    },
                    ServiceStatistics = new
                    {
                        TotalServices = totalRooms + totalFlights + totalCars + totalTours,
                        Rooms = totalRooms,
                        Flights = totalFlights,
                        Cars = totalCars,
                        Tours = totalTours
                    },
                    BookingStatistics = new
                    {
                        TotalBookings = totalBookings,
                        PendingBookings = pendingBookings,
                        ConfirmedBookings = confirmedBookings,
                        CancelledBookings = cancelledBookings,
                        BookingsByType = new
                        {
                            HotelBookings = hotelBookings,
                            FlightBookings = flightBookings,
                            CarBookings = carBookings,
                            TourBookings = tourBookings
                        }
                    },
                    Message = "Super admin dashboard data retrieved successfully"
                };

                return dashboardData;
            }
            catch (Exception ex)
            {
                return new { Success = false, Message = ex.Message };
            }
        }




    }
}
