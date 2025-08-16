using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications.RoomSpecs;

namespace TravelBooking.Service.Services
{
    public class RoomService : IRoomService
    {
        private readonly IGenericRepository<Booking> _bookingRepo;
        public RoomService(IGenericRepository<Booking> bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

        public async Task<List<DateRange>> GetAvailableDateRanges(int roomId, DateTime start, DateTime end)
        {
            var spec = new BookingByRoomAndDateRangeSpec(roomId, start, end);
            var bookings = await _bookingRepo.GetAllWithSpecAsync(spec);

            // Only consider confirmed bookings for availability
            var confirmedBookings = bookings
                .Where(b => b.Status == Status.Confirmed)
                .OrderBy(b => b.StartDate)
                .ToList();

            var availableRanges = new List<DateRange>();
            DateTime current = start;

            foreach (var booking in confirmedBookings)
            {
                if (current < booking.StartDate)
                {
                    var gapEnd = booking.StartDate.AddDays(-1);
                    if (current <= gapEnd)
                    {
                        availableRanges.Add(new DateRange(current, gapEnd));
                    }
                }

                // Move current pointer past this booking
                current = booking.EndDate.AddDays(1);

                if (current > end)
                    break;
            }

            // Add final range if there's still time left
            if (current <= end)
            {
                availableRanges.Add(new DateRange(current, end));
            }

            return availableRanges;
        }
    }
}