using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications.RoomSpecs
{
    // BookingByRoomAndDateRangeSpec.cs
    public class BookingByRoomAndDateRangeSpec : BaseSpecifications<Booking>
    {
        public BookingByRoomAndDateRangeSpec(int roomId, DateTime startDate, DateTime endDate)
            : base(b => b.RoomId == roomId &&
                        b.EndDate >= startDate &&
                        b.StartDate <= endDate)
        {
            AddOrderBy(b => b.StartDate);
            AddInclude(b => b.Room);
        }
    }
}
