using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.Core.Models;

namespace TravelBooking.Core.Repository.Contract
{
    public interface IPaymentService
    { 
        Task<Booking?> CreatePaymentIntent(int bookingId);

    }

}