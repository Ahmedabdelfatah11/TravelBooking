using Microsoft.Extensions.Configuration;
using Stripe;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications.BookingWithIncludesSpecification;

namespace TravelBooking.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<Booking> _bookingRepo;

        public PaymentService(IConfiguration configuration, IGenericRepository<Booking> bookingRepo)
        {
            _configuration = configuration;
            _bookingRepo = bookingRepo;
        }

        public async Task<Booking?> CreatePaymentIntent(int bookingId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var spec = new BookingWithIncludesSpecification(bookingId);
            var booking = await _bookingRepo.GetWithSpecAsync(spec);

            if (booking == null)
                return null;

            // if the booking is already paid, return null
            if (booking.Payment != null && booking.Payment.PaymentStatus == PaymentStatus.Paid)
            {
                return null;
            }

            decimal total = CalculateTotalPrice(booking);
            if (total <= 0)
                return null;

            // if the booking already has a PaymentIntentId, retrieve it
            if (!string.IsNullOrEmpty(booking.PaymentIntentId))
            {
                var existing = await new PaymentIntentService().GetAsync(booking.PaymentIntentId);
                booking.ClientSecret = existing.ClientSecret;
                return booking;
            }

            // New PaymentIntent creation
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(total * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },
                Metadata = new Dictionary<string, string>
                {
                    { "BookingId", booking.Id.ToString() },
                    { "CustomerId", booking.UserId ?? "" }
                }
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);
             
            // Save the PaymentIntent ID and ClientSecret in the booking
            booking.PaymentIntentId = intent.Id;
            booking.ClientSecret = intent.ClientSecret;
             
            // create a new Payment object and set its properties
            booking.Payment = new Payment
            {
                Amount = total,
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.Pending,
                PaymentMethod = "card",
                TransactionId = intent.Id
            };

            await _bookingRepo.Update(booking);

            return booking;
        }

        private decimal CalculateTotalPrice(Booking booking)
        {
            decimal total = 0;
            int duration = (int)(booking.EndDate - booking.StartDate).TotalDays;
            if (duration <= 0) duration = 1;

            switch (booking.BookingType)
            {
                case BookingType.Room:
                    total = (booking.Room?.Price ?? 0) * duration;
                    break;
                case BookingType.Car:
                    total = (booking.Car?.Price ?? 0) * duration;
                    break;
                case BookingType.Flight:
                    total = booking.SeatClass switch
                    {
                        SeatClass.Economy => booking.Flight?.EconomyPrice ?? 0,
                        SeatClass.Business => booking.Flight?.BusinessPrice ?? 0,
                        SeatClass.FirstClass => booking.Flight?.FirstClassPrice ?? 0,
                        _ => 0
                    };
                    break;

                case BookingType.Tour:
                    total = booking.Tour?.Price ?? 0;
                    break;
            }

            return total;
        }
    }
}
