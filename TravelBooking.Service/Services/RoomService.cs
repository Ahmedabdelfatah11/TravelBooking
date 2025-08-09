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
        public RoomService(IGenericRepository<Booking> bookingRepo) {
            _bookingRepo = bookingRepo ;
        }
        
        public async Task<List<DateRange>> GetAvailableDateRanges(int roomId, DateTime start, DateTime end)
        {
            var spec = new BookingByRoomAndDateRangeSpec(roomId, start, end);
            var bookings = await _bookingRepo.GetAllWithSpecAsync(spec); 

            var availableRanges = new List<DateRange>();
            DateTime current = start;

            foreach (var booking in bookings)
            {
                if (current < booking.StartDate)
                {
                    availableRanges.Add(new DateRange(current, booking.StartDate.AddDays(-1)));
                }
                current = booking.EndDate.AddDays(1); // نبدأ من اليوم التالي
            }

            // بعد آخر حجز
            if (current <= end)
            {
                availableRanges.Add(new DateRange(current, end));
            }

            return availableRanges;
        }
    }
}
