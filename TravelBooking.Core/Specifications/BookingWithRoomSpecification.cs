using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications
{
    public class BookingWithRoomSpecification : BaseSpecifications<Booking>
    {
        public BookingWithRoomSpecification(int hotelId)
            : base(b => b.Room.HotelId == hotelId && b.Status == Status.Confirmed)
        {
            AddInclude(b => b.Room);
        }
    }
}
