using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TravelBooking.Core.Models;
using TravelBooking.Core.Specifications;

public class BookingSpecification : BaseSpecifications<Booking>
{
    public BookingSpecification(int id) : base(b => b.Id == id)
    {
        // Include related data if any, for example User:
        Includes.Add(b => b.User);
        Includes.Add(b=>b.Payment);
        // Add more includes if needed
    }

    public BookingSpecification()
    {
        // For listing with no filter
    }
}

