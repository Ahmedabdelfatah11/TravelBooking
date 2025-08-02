using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Specifications.BookingWithIncludesSpecification
{
    public class BookingWithIncludesSpecification : BaseSpecifications<Booking>
    {
        public BookingWithIncludesSpecification(int bookingId)
     : base(b => b.Id == bookingId)
        {
            AddInclude(b => b.User);
            AddInclude(b => b.Room);
            AddInclude(b => b.Car);
            AddInclude(b => b.Flight);
            AddInclude(b => b.Tour);
        }
    }

}
