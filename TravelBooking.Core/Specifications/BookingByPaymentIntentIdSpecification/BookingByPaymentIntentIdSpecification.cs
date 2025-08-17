using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TravelBooking.Core.Models;
using TravelBooking.Core.Specifications;

public class BookingByPaymentIntentIdSpecification : BaseSpecifications<Booking>
{
    public BookingByPaymentIntentIdSpecification(string paymentIntentId)
        : base(b => b.PaymentIntentId == paymentIntentId)
    {
        AddInclude(b => b.Payment);
        AddInclude(b => b.Flight);
        AddInclude(b => b.Room);
        AddInclude(b => b.Car);
        Includes.Add(b => b.BookingTickets);
        AddInclude("BookingTickets.Ticket");
        AddInclude(b => b.Tour);

    }
}

